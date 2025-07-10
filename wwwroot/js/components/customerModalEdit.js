import { httpGet, confirmAction, resetNotifs, showErrorModal, showErrorSimpleModal, httpPut, httpDelete } from "../utils.js";

let inputs = {
	lname: null,
	fname: null,
	email: null,
	contactNo: null,
	address: null,
	hdnCustId: null
};

const notifs = {
	LastName: null,
	FirstName: null,
	Email: null,
	ContactNo: null,
	Address: null,
};

export function initModalNotifs(
	lnameId,
	fnameId,
	emailId,
	contactNoId,
	addressId
) {
	notifs.LastName = $(`#${lnameId}`);
	notifs.FirstName = $(`#${fnameId}`);
	notifs.Email = $(`#${emailId}`);
	notifs.ContactNo = $(`#${contactNoId}`);
	notifs.Address = $(`#${addressId}`);

	console.log(notifs);
}

export function initModal(
	lnameId,
	fnameId,
	emailId,
	contactNoId,
	addressId,
	hdnCustElemId
) {
	inputs.lname = $(`#${lnameId}`);
	inputs.fname = $(`#${fnameId}`);
	inputs.email = $(`#${emailId}`);
	inputs.contactNo = $(`#${contactNoId}`);
	inputs.address = $(`#${addressId}`);
	inputs.hdnCustId = $(`#${hdnCustElemId}`);

	console.log(inputs);
}

function onEdit()
{
	const _url = `${window.custUrl}?id=${inputs.hdnCustId.val()}`

	httpGet(
		_url,
		"Customer Not Found",
		"There was a problem while fetching the customer",
		(response) => {
			const result = response.result;
			inputs.lname.val(result.lastName);
			inputs.fname.val(result.firstName);
			inputs.email.val(result.email);
			inputs.contactNo.val(result.contactNo);
			inputs.address.val(result.address);
		}
	);
}


function edit(e, errorTitle, errorDescription, notifList) {
	if (!e.isConfirmed) return;

	resetNotifs(notifs);

	const dto = {
		Id: inputs.hdnCustId.val(),
		FirstName: inputs.fname.val(),
		LastName: inputs.lname.val(),
		Email: inputs.email.val(),
		ContactNo: inputs.contactNo.val(),
		Address: inputs.address.val(),
	};

	httpPut(
		window.custUrl,
		dto,
		'modal-edit-customer',
		errorTitle,
		errorDescription,
		notifList,
		() => displaySpinner(),
		() => hideSpinner(),
	);
}

$(document).ready(function ()
{
	$('#btn-edit-customer').click(function (e) {
		onEdit();
	});

	$('#form-edit-customer').submit(function (e) {
		e.preventDefault();
		confirmAction(
			'Edit',
			'Edit Customer?',
			'This will edit the current customer with the provided details!',
			"Customer Edit Failed",
			"There was a problem while editing the customer.",
			notifs,
			edit
		);
	});
});