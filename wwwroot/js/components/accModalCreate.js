import { accountCreationRequestDto } from "../dtos/accountCreationRequestDto.js";
import { handleError, confirmAction, httpPost } from "../utils.js";

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

function submit(e, errorTitle, errorDescription, notifList)
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
      'Create Account?',
      'This will create current account with the provided details!',
      'Account Creation Failed',
      'There was a problem while creating the account.',
      notifs,
      submit
    );
  });
});
