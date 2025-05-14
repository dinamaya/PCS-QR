import { spCreationRequestDto } from '../dtos/spCreationRequestDto.js';
import { handleError, confirmAction } from "../utils.js";

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

function submit(e) {
  if (!e.isConfirmed) return;

  const dto = new spCreationRequestDto(
    inputs.spName.val(),
    inputs.companyName.val(),
    inputs.contactPerson.val(),
    inputs.contactNumber.val(),
    inputs.email.val()
  );

  $.post({
    url: window.baseUrl,
    contentType: 'application/json',
    data: JSON.stringify(dto),
    success: function (response) {
      const modalEl = $('#modal-create');
      const modalInstance = bootstrap.Modal.getInstance(modalEl);
      modalInstance.hide();

      setTimeout(() => {
        const url = new URL(window.location.href);

        url.searchParams.set('q', window.okCreateParam);
        window.location.href = url.toString();
      }, 300);
    },
    error: (error) => handleError(error, "Service Partner Creation Failed", "There was a problem while creating the service partner.", notifs)
  });
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
