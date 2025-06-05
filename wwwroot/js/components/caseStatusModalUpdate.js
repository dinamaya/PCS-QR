import { confirmAction, confirmAction2, handleError, resetNotifs, showErrorModal, showErrorSimpleModal, httpPut, httpDelete } from "../utils.js";

let inputs = {
	currStatus: null,
	status: null,
	statSelect: null,
	comments: null,
	commentContainer: null,
	hdnCaseId: null
};

let notifs = {
	StatusId: null,
	Comments: null,
};

let $linkCase = $(`#link-case`);

export function initModal(
	currStatusId,
	statusChoiceInstance,
	statusSelectId,
	commentsId,
	hdnCaseIdElemId,
) {
	inputs.currStatus = $(`#${currStatusId}`);
	inputs.status = statusChoiceInstance;
	inputs.statSelect = $(`#${statusSelectId}`);

	inputs.comments = $(`#${commentsId}`);
	inputs.hdnCaseId = $(`#${hdnCaseIdElemId}`);
	inputs.commentContainer = inputs.comments.parent().parent().parent().parent();

	console.log(inputs);
}

export function initModalNotifs(
	statusId,
	commentsId
) {
	notifs.StatusId = $(`#${statusId}`);
	notifs.Comments = $(`#${commentsId}`);

	console.log(notifs);
}

export function onEdit(button) {
		const row = button.closest("tr");
	const caseId = row.dataset.caseId;

	inputs.hdnCaseId.val(caseId);
	const _url = `${window.baseUrl}/status?caseId=${caseId}`

	$.ajax({
		url: _url,
		method: 'GET',
		dataType: 'json',
		success: function (data) {
			const result = data.result;

			if (data && result)
			{
				reset();
				inputs.status.setChoices(result.availableStatus, 'value', 'label', true);
				inputs.status.setChoiceByValue("", true);
				inputs.currStatus.val(result.currentStatus);
				$linkCase.attr('href', `/Cases/Update/${caseId}`);
			}
			else {
				showErrorModal("No Result", "Case Not Found", "There was a problem while fetching the case.", notifs);
			}
		},
		error: (error) => handleError(error, "Case Fetching Failed", "There was a problem while fetching the case.", notifs)
	});
}

const reset = () => {
	inputs.commentContainer.addClass("d-none");
	inputs.status.clearStore();
	inputs.status.clearChoices();
	inputs.status.clearInput();
	inputs.status.removeActiveItems();
}

function edit(e, errorTitle, errorDescription, notifList) {
	if (!e.isConfirmed) return;

	resetNotifs(notifs);

	const dto = {
		CaseId: inputs.hdnCaseId.val(),
		Comments: inputs.comments.val(),
		StatusId: inputs.status.getValue().value,
	};

	const _url = window.baseUrl + "/status"

	httpPut(
		_url,
		dto,
		'modal-update-stat',
		errorTitle,
		errorDescription,
		notifList,
		() => displaySpinner(),
		() => hideSpinner(),
	);
}

function checkIsCommentable(selectedStatus) {
	try {
		const _url = `/api/ops/status/commentable?id=${encodeURIComponent(selectedStatus)}`;
		console.log("Url:", _url);

		fetch(_url)
			.then(response => response.json())
			.then(data =>
			{
				if (data.result)
					inputs.commentContainer.removeClass("d-none");
				else
					inputs.commentContainer.addClass("d-none");
			})
			.catch(ex => showErrorSimpleModal(ex.message, "Fetch error"));
	}
	catch (ex)
	{
		showErrorSimpleModal("Exception in checkIsCommentable:", ex);
	}
}

$(document).ready(function () {
	$('#form-update-stat').submit(function (event) {
		event.preventDefault();
		confirmAction(
			'Update',
			'Update Case Status?',
			'This will update the current case status with the provided details!',
			"Case Status Update Failed",
			"There was a problem while updating the case status.",
			notifs,
			edit
		);
	});


	$(inputs.statSelect).on("change", function () {
		checkIsCommentable(this.value);
	});
});