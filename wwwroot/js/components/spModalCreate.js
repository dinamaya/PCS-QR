import { spCreationRequestDto } from '../dtos/spCreationRequestDto.js';
import { httpPost, confirmAction } from "../utils.js";

let inputs = {
  spName: null,
  companyName: null,
  contactPerson: null,
  contactNumber: null,
  email: null,
};


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
	emailId
) {
	inputs.spName = $(`#${spNameId}`);
  inputs.companyName = $(`#${companyNameId}`);
  inputs.contactPerson = $(`#${contactPersonId}`);
  inputs.contactNumber = $(`#${contactNumberId}`);
  inputs.email = $(`#${emailId}`);

  console.log(inputs);
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

function submit(e, errorTitle, errorDescription, notifList) {
  if (!e.isConfirmed) return;

  const dto = new spCreationRequestDto(
    inputs.spName.val(),
    inputs.companyName.val(),
    inputs.contactPerson.val(),
    inputs.contactNumber.val(),
    inputs.email.val()
  );

  httpPost(
    window.baseUrl,
    dto,
    'modal-create',
    errorTitle,
    errorDescription,
    notifList,
    () => displaySpinner(),
    () => hideSpinner(),
  );
}

$(document).ready(function () {
  $('#form-create').submit(function (event) {
    event.preventDefault();
    confirmAction(
      'Create',
      'Create Service Partner?',
      'This will create current service partner with the provided details!',
      'Service Partner Creation Failed',
      'There was a problem while creating the service partner.',
      notifs,
      submit
    );
  });
});
