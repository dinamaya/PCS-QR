
let inputs = {
  name: null,
  isCommentable: null,
  hdnId: null,
};

export function initModal(
  nameId,
  isCommentableId,
  hdnElemId
) {
  inputs.name = $(`#${nameId}`);
  inputs.isCommentable = $(`#${isCommentableId}`);
  inputs.hdnId = $(`#${hdnElemId}`);
}

function edit() {
	try {
		const dto = {
			Id: inputs.hdnId.val(),
			Name: inputs.name.val(),
			IsCommentable: inputs.isCommentable.is(":checked"),
		};

		console.log("Dto Sent:", dto);

		fetch(window.baseUrl, {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(dto)
		})
			.then(response => response.json())
			.then(data => {
				if (data.isSuccess) {
					console.log("Edit successful:", data);
					$("#modal-edit").modal("hide");

					setTimeout(() => {
						location.reload();
					}, 500);
				}
				else {
					console.error("Edit failed:", data.message);
					alert("Edit failed: " + data.message);
				}
				console.log("Edit successful:", data);
			})
			.catch(ex => {
				console.error("Error during PUT request:", ex);
				alert("Something went wrong.");
			});
	}
	catch (ex) {
		console.error("Exception in submitEdit:", ex);
	}
}

export function onEdit(button) {
  const row = button.closest("tr");
  const id = row.dataset.statId;

  inputs.hdnId.val(id);
  const _url = `${window.baseUrl}?id=${id}`

  try {
    fetch(_url)
      .then(response => response.json())
      .then(data => {
        const result = data.result;
        if (data && result) {
          inputs.name.val(result.name);
          inputs.isCommentable.prop("checked", result.isCommentable);
        }
        else {
          console.error("No valid data received from server.");
        }
      })
      .catch(ex => console.error("Fetch error:", ex));
  }
  catch (ex) {
    console.error(ex);
  }
}

$(document).ready(function () {
	$('#form-edit').submit(function (event) {
		event.preventDefault();
		edit();
	});
});