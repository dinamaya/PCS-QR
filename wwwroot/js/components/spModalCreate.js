import { spCreationRequestDto } from '../dtos/spCreationRequestDto.js';

let spName = null;
let companyName = null;
let contactPerson = null;
let contactNumber = null;
let email = null;
let url = "";

export function initModal(
  urlCreate,
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
  url = urlCreate;
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
    url: url,
    contentType: 'application/json',
    data: JSON.stringify(dto),
    success: function (response) {
      console.log("Submission successful:", response);
      alert("Submission successful!");
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
