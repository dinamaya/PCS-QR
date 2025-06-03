import { resetNotifs, httpGet, httpPut, httpDelete, confirmAction, confirmAction2 } from "../utils.js"

let inputs = {
  name: null,
  isCommentable: null,
  hdnId: null,
};

const notifs = {
	Name: null,
};

export function initModal(
  nameId,
  isCommentableId,
  hdnElemId
) {
  inputs.name = $(`#${nameId}`);
  inputs.isCommentable = $(`#${isCommentableId}`);
  inputs.hdnId = $(`#${hdnElemId}`);
}

export function initModalNotifs(
	nameId,
) {
	notifs.Name = $(`#${nameId}`);

	console.log(notifs);
}

function edit(e, errorTitle, errorDescription, notifList)
{
	if (!e.isConfirmed) return;

	resetNotifs(notifList);

	const dto = {
		Id: inputs.hdnId.val(),
		Name: inputs.name.val(),
		IsCommentable: inputs.isCommentable.is(":checked"),
	};

	httpPut(
		window.baseUrl,
		dto,
		'modal-edit',
		errorTitle,
		errorDescription,
		notifList
	);
}

function deactivate(e, errorTitle, errorDescription) {
	if (!e.isConfirmed) return;

	const deleteUrl = new URL(window.baseUrl, window.location.origin);
	deleteUrl.searchParams.set('id', inputs.hdnId.val());

	httpDelete(
		deleteUrl,
		'modal-edit',
		errorTitle,
		errorDescription
	);
}

export function onEdit(button) {
  const row = button.closest("tr");
  const id = row.dataset.statId;

  inputs.hdnId.val(id);
  const _url = `${window.baseUrl}?id=${id}`

	httpGet(
		_url,
		"Status Details Not Found",
		"There was a problem while fetching the status details.",
		(response) => {
			var result = response.result;

			inputs.name.val(result.name);
			inputs.isCommentable.prop("checked", result.isCommentable);
		}
	);
}

$(document).ready(function () {
	$('#form-edit').submit(function (event) {
		event.preventDefault();
		confirmAction(
			'Edit',
			'Edit Status?',
			'This will edit the current status with the provided details!',
			"Status Edit Failed",
			"There was a problem while editing the status details.",
			notifs,
			edit
		);
	});


	$('#btn-delete').click(function (event) {
		event.preventDefault();
		confirmAction2(
			'Delete',
			'Delete Status?',
			'This will delete the current status with the provided details!',
			"Status Delete Failed",
			"There was a problem while deleting the status details.",
			deactivate
		);
	});
});