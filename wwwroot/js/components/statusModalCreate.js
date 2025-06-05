import { statusCreationRequestDto } from '../dtos/statusCreationRequestDto.js';
import { httpPost, confirmAction } from '../utils.js';

let inputs = {
  name: null,
  isCommentable: null,
};

const notifs = {
  Name: null,
  IsCommentable: null
};

export function initModal(
  nameId,
  isCommentableId,
) {
  inputs.name = $(`#${nameId}`);
  inputs.isCommentable = $(`#${isCommentableId}`);
}

export function initModalNotifs(
  nameId,
  isCommentableId,
) {
  notifs.Name = $(`#${nameId}`);
  notifs.IsCommentable = $(`#${isCommentableId}`);

  console.log(notifs);
}


function submit(e) {
  if (!e.isConfirmed) return;

  const dto = new statusCreationRequestDto(
    inputs.name.val(),
    inputs.isCommentable.is(":checked"),
  );

  httpPost(
    window.baseUrl,
    dto,
    "modal-create",
    "Status Creation Failed",
    "There was a problem while creating the status details.",
    notifs,
    () => displaySpinner(),
    () => hideSpinner(),
  );
}

$(document).ready(function () {
  $('#form-create').submit(function (event) {
    event.preventDefault();
    confirmAction(
      'Create',
      'Create Status?',
      'This will create a new status with the provided details!',
      'Status Creation Failed',
      'There was a problem while creating the status details.',
      notifs,
      submit
    );
  });
});
