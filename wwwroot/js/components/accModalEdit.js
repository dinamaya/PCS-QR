import { accountCreationRequestDto } from "../dtos/accountCreationRequestDto.js";

let inputs = {
  lname: null,
  fname: null,
  uname: null,
  email: null,
  pass1: null,
  pass2: null,
  passType: null,
  type: null,
	hdnAccId: null
};

let _radioGroupId = "";

export function initModal(
  lnameId,
  fnameId,
  unameId,
  emailId,
  pass1Id,
  pass2Id,
  typeChoiceInstance,
	hdnAccIdElemId,
	radioGroupId
) {
  inputs.lname = $(`#${lnameId}`);
  inputs.fname = $(`#${fnameId}`);
  inputs.uname = $(`#${unameId}`);
  inputs.email = $(`#${emailId}`);
  inputs.pass1 = $(`#${pass1Id}`);
  inputs.pass2 = $(`#${pass2Id}`);
	inputs.type = typeChoiceInstance;

	inputs.hdnAccId = $(`#${hdnAccIdElemId}`);
	_radioGroupId = radioGroupId;

  console.log(inputs);
}

export function onEdit(button) {
  try
  {
	  const row = button.closest("tr");
	  const accId = row.dataset.accId;

	  inputs.hdnAccId.val(accId);
	  const _url = `${window.baseUrl}?id=${accId}`
    console.log(_url);

		fetch(_url)
			.then(response => response.json())
			.then(data => {
				const result = data.result;
				console.log("Result: ", result);

				if (data && result) {
					inputs.lname.val(result.lastName);
					inputs.fname.val(result.firstName);
					inputs.uname.val(result.username);
					inputs.email.val(result.email);
					inputs.type.setChoiceByValue(result.type);
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

function edit()
{
	try {
		const dto = {
			Id: inputs.hdnAccId.val(),
			FirstName: inputs.fname.val(),
			LastName: inputs.lname.val(),
			Username: inputs.uname.val(),
			Email: inputs.email.val(),
			AccountType: inputs.type.getValue().value,
			PasswordResetType: getSelectedPasswordOption(),
			Password: inputs.pass1.val(),
			RetypePass: inputs.pass2.val(),
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

function getSelectedPasswordOption() {
	const selectedRadio = document.querySelector(`input[name="${_radioGroupId}"]:checked`);

	if (selectedRadio) {
		const label = document.querySelector(`label[for="${selectedRadio.id}"]`);
		return label ? label.textContent.trim() : '';
	}

	return '';
}

$(document).ready(function () {
	$('#form-edit').submit(function (event) {
		event.preventDefault();
		edit();
	});
});