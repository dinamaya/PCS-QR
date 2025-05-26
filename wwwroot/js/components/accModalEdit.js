import { confirmAction, confirmAction2, handleError, resetNotifs, showErrorModal, showErrorSimpleModal, httpPut, httpDelete } from "../utils.js";

let inputs = {
  lname: null,
  fname: null,
  uname: null,
  email: null,
  pass1: null,
  pass2: null,
  passType: null,
  type: null,
	hdnAccId: null
};

const notifs = {
	LastName: null,
	FirstName: null,
	Username: null,
	Email: null,
	Password: null,
	RetypePass: null,
	AccountType: null,
};

let _radioGroupName = "";

export function initModalNotifs(
	lnameId,
	fnameId,
	unameId,
	emailId,
	pass1Id,
	pass2Id,
	typeId)
{
	notifs.LastName = $(`#${lnameId}`);
	notifs.FirstName = $(`#${fnameId}`);
	notifs.Username = $(`#${unameId}`);
	notifs.Email = $(`#${emailId}`);
	notifs.Password = $(`#${pass1Id}`);
	notifs.RetypePass = $(`#${pass2Id}`);
	notifs.AccountType = $(`#${typeId}`);

	console.log(notifs);
}

export function initModal(
  lnameId,
  fnameId,
  unameId,
  emailId,
  pass1Id,
  pass2Id,
  typeChoiceInstance,
	hdnAccIdElemId,
	radioGroupName
)
{
  inputs.lname = $(`#${lnameId}`);
  inputs.fname = $(`#${fnameId}`);
  inputs.uname = $(`#${unameId}`);
  inputs.email = $(`#${emailId}`);
  inputs.pass1 = $(`#${pass1Id}`);
  inputs.pass2 = $(`#${pass2Id}`);
	inputs.type = typeChoiceInstance;

	inputs.hdnAccId = $(`#${hdnAccIdElemId}`);
	_radioGroupName = radioGroupName;
}

export function onEdit(button)
{
	resetNotifs(notifs);
	const row = button.closest("tr");
	const accId = row.dataset.accId;

	inputs.hdnAccId.val(accId);
	const _url = `${window.baseUrl}?id=${accId}`

	$.ajax({
		url: _url,
		method: 'GET',
		dataType: 'json',
		success: function (data) {
			const result = data.result;

			if (data && result) {
				inputs.lname.val(result.lastName);
				inputs.fname.val(result.firstName);
				inputs.uname.val(result.username);
				inputs.email.val(result.email);
				inputs.type.setChoiceByValue(result.type);
			}
			else {
				showErrorModal("No Result", "Account Not Found", "There was a problem while fetching the account.", notifs);
			}
		},
		error: (error) => handleError(error, "Account Fetching Failed", "There was a problem while fetching the account.", notifs)
	});
}

function edit(e, errorTitle, errorDescription, notifList) {
	if (!e.isConfirmed) return;

	resetNotifs(notifs);

	const dto = {
		Id: inputs.hdnAccId.val(),
		FirstName: inputs.fname.val(),
		LastName: inputs.lname.val(),
		Username: inputs.uname.val(),
		Email: inputs.email.val(),
		AccountType: inputs.type.getValue().value,
		PasswordResetType: getSelectedPasswordOption(),
		Password: inputs.pass1.val(),
		RetypePass: inputs.pass2.val(),
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

function deactivate(e, errorTitle, errorDescription)
{
	if (!e.isConfirmed) return;

	const deleteUrl = new URL(window.baseUrl, window.location.origin);
	deleteUrl.searchParams.set('id', inputs.hdnAccId.val());

	httpDelete(
		deleteUrl,
		'modal-edit',
		errorTitle,
		errorDescription
	);
}

function getSelectedPasswordOption() 
{
	const selectedRadio = document.querySelector(`input[name="${_radioGroupName}"]:checked`);

	console.log(selectedRadio)

	if (selectedRadio) {
		const label = document.querySelector(`label[for="${selectedRadio.id}"]`);
		return label ? label.textContent.trim() : '';
	}

	return '';
}


$(document).ready(function () {
	$('#form-edit').submit(function (event) {
		event.preventDefault();
		confirmAction(
			'Edit',
			'Edit Account?',
			'This will edit the current account with the provided details!',
			"Account Edit Failed",
			"There was a problem while editing the account.",
			notifs,
			edit
		);
	});

	$('#btn-delete').click(function (event) {
		event.preventDefault();
		confirmAction2(
			'Delete',
			'Delete Account?',
			'This will delete the current account!',
			"Account Delete Failed",
			"There was a problem while deleting the account.",
			deactivate
		);
	});
});