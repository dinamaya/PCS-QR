import { accountCreationRequestDto } from "../dtos/accountCreationRequestDto.js";
import { displayErrors, showErrorModal, handleError } from "../utils.js";

let inputs = {
  lname: null,
  fname: null,
  uname: null,
  email: null,
  pass1: null,
  pass2: null,
  type: null,
};

const notifs = {
  LastName: null,
  FirstName: null,
  Username: null,
  Email: null,
  Password: null,
  RetypePass: null,
  Type: null,
};

export function initModalNotifs(lnameId,
  fnameId,
  unameId,
  emailId,
  pass1Id,
  pass2Id,
  typeId) {

  notifs.LastName = $(`#${lnameId}`);
  notifs.FirstName = $(`#${fnameId}`);
  notifs.Username = $(`#${unameId}`);
  notifs.Email = $(`#${emailId}`);
  notifs.Password = $(`#${pass1Id}`);
  notifs.RetypePass = $(`#${pass2Id}`);
  notifs.Type = $(`#${typeId}`);
}

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

function post()
{
  Swal.mixin({
    customClass: {
      confirmButton: 'btn bg-gradient-success',
      cancelButton: 'btn bg-gradient-danger'
    },
    buttonsStyling: !1
  })
    .fire({
      title: 'Create Account?',
      text: 'This will create current account with the provided details!',
      icon: 'question',
      confirmButtonText: 'Create',
      cancelButtonText: 'Cancel',
      reverseButtons: !0,
      showCancelButton: !0
    })
    .then(submit)
    .catch(e => {
      showErrorModal(e.message, "Account Creation Failed", "There was a problem while creating the account.", notifs);
    });
}

function submit(e)
{
  if (!e.isConfirmed) return;

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
        const url = new URL(window.location.href);

        url.searchParams.set('q', window.okEditParam);
        window.location.href = url.toString();
      }, 300);
    },
    error: (error) => handleError(error, "Account Creation Failed", "There was a problem while creating the account.", notifs)
  });
}


$(document).ready(function () {
  $('#form-create').submit(function (event) {
    event.preventDefault();
    post();
  });
});
