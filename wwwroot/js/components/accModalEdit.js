import { accountCreationRequestDto } from "../dtos/accountCreationRequestDto.js";

let inputs = {
  lname: null,
  fname: null,
  uname: null,
  email: null,
  pass1: null,
  pass2: null,
  type: null,
	hdnAccId: null
};

export function initModal(
  lnameId,
  fnameId,
  unameId,
  emailId,
  pass1Id,
  pass2Id,
  typeChoiceInstance,
	hdnAccIdElemId
) {
  inputs.lname = $(`#${lnameId}`);
  inputs.fname = $(`#${fnameId}`);
  inputs.uname = $(`#${unameId}`);
  inputs.email = $(`#${emailId}`);
  inputs.pass1 = $(`#${pass1Id}`);
  inputs.pass2 = $(`#${pass2Id}`);
	inputs.type = typeChoiceInstance;

	inputs.hdnAccId = $(`#${hdnAccIdElemId}`);

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
