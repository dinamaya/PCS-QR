import { statusCreationRequestDto } from '../dtos/statusCreationRequestDto.js';

let inputs = {
  name: null,
  isCommentable: null,
};

export function initModal(
  nameId,
  isCommentableId,
) {
  inputs.name = $(`#${nameId}`);
  inputs.isCommentable = $(`#${isCommentableId}`);
}

function post() {
  const dto = new statusCreationRequestDto(
    inputs.name.val(),
    inputs.isCommentable.is(":checked"),
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
