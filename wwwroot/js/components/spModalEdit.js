import { confirmAction, confirmAction2, handleError, resetNotifs, showErrorModal, httpPut, httpDelete } from "../utils.js";

let inputs = {
	spName : null,
	companyName : null,
	contactPerson : null,
	contactNumber : null,
	email: null,
	hdnSpId: null,
	url : "",
}

const notifs = {
	Name: null,
	CompanyName: null,
	ContactPerson: null,
	ContactNumber: null,
	Email: null,
};

export function initModal(
	spNameId,
	companyNameId,
	contactPersonId,
	contactNumberId,
	emailId,
	hdnSpIdElemId
) {
	inputs.url = window.baseUrl;
	inputs.spName = $(`#${spNameId}`);
	inputs.companyName = $(`#${companyNameId}`);
	inputs.contactPerson = $(`#${contactPersonId}`);
	inputs.contactNumber = $(`#${contactNumberId}`);
	inputs.email = $(`#${emailId}`);

	inputs.hdnSpId = $(`#${hdnSpIdElemId}`);
}

export function initModalNotifs(
  spNameId,
  companyNameId,
  contactPersonId,
  contactNumberId,
  emailId
) {
  notifs.Name = $(`#${spNameId}`);
  notifs.CompanyName = $(`#${companyNameId}`);
  notifs.ContactPerson = $(`#${contactPersonId}`);
  notifs.ContactNumber = $(`#${contactNumberId}`);
  notifs.Email = $(`#${emailId}`);

  console.log(notifs);
}

export function onEdit(button)
{
	resetNotifs(notifs);
	const row = button.closest("tr");
	const spId = row.dataset.spId;

	inputs.hdnSpId.val(spId);
	const _url = `${inputs.url}?id=${spId}`

	$.ajax({
		url: _url,
		method: 'GET',
		dataType: 'json',
		success: function (data) {
			const result = data.result;

			if (data && result) {
				inputs.spName.val(result.name);
				inputs.companyName.val(result.companyName);
				inputs.contactPerson.val(result.contactPerson);
				inputs.contactNumber.val(result.contactNumber);
				inputs.email.val(result.email);
			}
			else {
				showErrorModal("No Result", "Service Partner Not Found", "There was a problem while fetching the service partner.", notifs);
			}
		},
		error: (error) => handleError(error, "Service Partner Fetching Failed", "There was a problem while fetching the service partner.", notifs)
	});
}

export function resetModal()
{
	setTimeout(() => {
		inputs.spName.val("");
		inputs.companyName.val("");
		inputs.contactPerson.val("");
		inputs.contactNumber.val("");
		inputs.email.val("");
		inputs.hdnSpId.val("");
	}, 800);
}

function edit(e, errorTitle, errorDescription, notifList)
{
	if (!e.isConfirmed) return;

	resetNotifs(notifs);

	const dto = {
		id: inputs.hdnSpId.val(),
		name: inputs.spName.val(),
		companyName: inputs.companyName.val(),
		contactPerson: inputs.contactPerson.val(),
		contactNumber: inputs.contactNumber.val(),
		email: inputs.email.val()
	};

	httpPut(
		inputs.url,
		dto,
		'modal-edit',
		errorTitle,
		errorDescription,
		notifList,
		() => displaySpinner(),
		() => hideSpinner(),
	);
}

function deactivate(e, errorTitle, errorDescription) {
	if (!e.isConfirmed) return;

	const deleteUrl = new URL(inputs.url, window.location.origin);
	deleteUrl.searchParams.set('id', inputs.hdnSpId.val());

	httpDelete(
		deleteUrl,
		'modal-edit',
		errorTitle,
		errorDescription
	);
}

$(document).ready(function () {
	$('#form-edit').submit(function (event) {
		event.preventDefault();
		confirmAction(
			'Edit',
			'Edit Service Partner?',
			'This will edit the current service partner with the provided details!',
			"Service Partner Edit Failed",
			"There was a problem while editing the service partner.",
			notifs,
			edit
		);
	});

	
	$('#btn-delete').click(function (event) {
		event.preventDefault();
		confirmAction2(
			'Delete',
			'Delete Service Partner?',
			'This will delete the current service partner!',
			"Service Partner Delete Failed",
			"There was a problem while deleting the service partner.",
			deactivate
		);
	});
});
