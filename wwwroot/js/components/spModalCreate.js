import { spCreationRequestDto } from '../dtos/spCreationRequestDto.js';

let spName = null;
let companyName = null;
let contactPerson = null;
let contactNumber = null;
let email = null;

export function initModal(
	spNameId,
	companyNameId,
	contactPersonId,
	contactNumberId,
	emailId
) {
	spName = $(`#${spNameId}`);
	companyName = $(`#${companyNameId}`);
	contactPerson = $(`#${contactPersonId}`);
	contactNumber = $(`#${contactNumberId}`);
  email = $(`#${emailId}`);
}

function post() {
  const dto = new spCreationRequestDto(
    spName.val(),
    companyName.val(),
    contactPerson.val(),
    contactNumber.val(),
    email.val()
  );

  $.post({
    url: window.baseUrl,
    contentType: 'application/json',
    data: JSON.stringify(dto),
    success: function (response) {
      const modalEl = document.getElementById('modal-create');
      const modalInstance = bootstrap.Modal.getInstance(modalEl);
      modalInstance.hide();

      setTimeout(() => {
        location.reload();
      }, 300);
    },
    error: function (error) {
      console.error("Submission failed:", error);
      alert("Submission failed!");
    }
  });
}

$(document).ready(function () {
  $('#form-create').submit(function (event) {
    event.preventDefault();
    post();
  });
});
