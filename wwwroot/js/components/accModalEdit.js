import { displayErrors, showErrorModal, handleError, resetNotifs, isNullOrEmpty, showErrorModal2 } from "../utils.js";

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

export function onEdit(button) {
  try
  {
	  const row = button.closest("tr");
	  const accId = row.dataset.accId;

	  inputs.hdnAccId.val(accId);
	  const _url = `${window.baseUrl}?id=${accId}`

		fetch(_url)
			.then(response => response.json())
			.then(data => {
				const result = data.result;

				if (data && result) {
					inputs.lname.val(result.lastName);
					inputs.fname.val(result.firstName);
					inputs.uname.val(result.username);
					inputs.email.val(result.email);
					inputs.type.setChoiceByValue(result.type);
				}
				else
				{
					showErrorModal(e.message, "Account Not Found", "There was a problem while fetching the account.", notifs);
				}
			})
			.catch(e => {
				showErrorModal(e.message, "Account Fetching Failed", "There was a problem while fetching the account.", notifs);
			})
	}
	catch (e) {
		showErrorModal(e.message, "Account Fetching Failed", "There was a problem while fetching the account.", notifs);
	}

}

function submit(e) {
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

	$.ajax({
		url: window.baseUrl,
		method: 'PUT',
		contentType: 'application/json',
		data: JSON.stringify(dto),
		success: function (response) {
			const modalEl = $('#modal-edit');
			const modalInstance = bootstrap.Modal.getInstance(modalEl);
			modalInstance.hide();

			setTimeout(() => {
				const url = new URL(window.location.href);
				url.searchParams.set('q', window.okEditParam);
				window.location.href = url.toString();
			}, 300);
		},
		error: (error) =>
			handleError(error, "Account Edit Failed", "There was a problem while editing the account.", notifs)
	});
}

function deleteData(e)
{
	if (!e.isConfirmed) return;

	const deleteUrl = new URL(window.baseUrl, window.location.origin);
	deleteUrl.searchParams.set('id', inputs.hdnAccId.val());

	console.log(deleteUrl);

	$.ajax({
		url: deleteUrl,
		method: 'DELETE',
		success: function (response)
		{
			const modalEl = $('#modal-edit');
			const modalInstance = bootstrap.Modal.getInstance(modalEl);
			modalInstance.hide();

			setTimeout(() => {
				const url = new URL(window.location.href);
				url.searchParams.set('q', window.okEditParam);
				window.location.href = url.toString();
			}, 300);
		},
		error: (error) =>
			handleError(error, "Account Edit Failed", "There was a problem while editing the account.", notifs)
	});
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

function put() {
	Swal.mixin({
		customClass: {
			confirmButton: 'btn bg-gradient-success',
			cancelButton: 'btn bg-gradient-danger'
		},
		buttonsStyling: !1
	})
		.fire({
			title: 'Edit Account?',
			text: 'This will edit the current account with the provided details!',
			icon: 'question',
			confirmButtonText: 'Create',
			cancelButtonText: 'Cancel',
			reverseButtons: !0,
			showCancelButton: !0
		})
		.then(submit)
		.catch(e => {
			showErrorModal(e.message, "Account Edit Failed", "There was a problem while creating the account.", notifs);
		});
}

function onDelete()
{
	Swal.mixin({
		customClass: {
			confirmButton: 'btn bg-gradient-success',
			cancelButton: 'btn bg-gradient-danger'
		},
		buttonsStyling: !1
	})
		.fire({
			title: 'Delete Account?',
			text: 'This will delete the current account!',
			icon: 'question',
			confirmButtonText: 'Delete',
			cancelButtonText: 'Cancel',
			reverseButtons: !0,
			showCancelButton: !0
		})
		.then(deleteData)
		.catch(e => {
			showErrorModal2(e.message, "Account Delete Failed", "There was a problem while deleting the account.");
		});
}


$(document).ready(function () {
	$('#form-edit').submit(function (event) {
		event.preventDefault();
		put();
	});

	$('#btn-delete').click(function (event) {
		event.preventDefault();
		onDelete();
	});
});