import { accountCreationRequestDto } from "../dtos/accountCreationRequestDto.js";

let inputs = {
  lname: null,
  fname: null,
  uname: null,
  email: null,
  pass1: null,
  pass2: null,
  type: null,
};

export function initModal(
  lnameId,
  fnameId,
  unameId,
  emailId,
  pass1Id,
  pass2Id,
  typeId
) {
  inputs.lname = $(`#${lnameId}`);
  inputs.fname = $(`#${fnameId}`);
  inputs.uname = $(`#${unameId}`);
  inputs.email = $(`#${emailId}`);
  inputs.pass1 = $(`#${pass1Id}`);
  inputs.pass2 = $(`#${pass2Id}`);
  inputs.type = $(`#${typeId}`);
}

function post() {
  const dto = new accountCreationRequestDto(
    inputs.fname.val(),
    inputs.lname.val(),
    inputs.uname.val(),
    inputs.email.val(),
    inputs.type.val(),
    inputs.pass1.val(),
    inputs.pass2.val(),
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
