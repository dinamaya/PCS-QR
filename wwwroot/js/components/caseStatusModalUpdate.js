
let inputs = {
	currStatus: null,
	status: null,
	hdnCaseId: null
};
export function initModal(
	currStatusId,
	statusChoiceInstance,
	hdnCaseIdElemId,
) {
	inputs.currStatus = $(`#${currStatusId}`);
	inputs.status = statusChoiceInstance;
	inputs.hdnCaseId = $(`#${hdnCaseIdElemId}`);

	console.log(inputs);
}

export function onEdit(button) {
	try {
		const row = button.closest("tr");
		const caseId = row.dataset.caseId;

		inputs.hdnCaseId.val(caseId);
		const _url = `${window.baseUrl}/status?caseId=${caseId}`
		console.log(_url);

		fetch(_url)
			.then(response => response.json())
			.then(data => {
				const result = data.result;
				console.log("Result: ", result);

				if (data && result) {
					reset();
					inputs.status.setChoices(result.availableStatus, 'value', 'label', true);
					inputs.status.setChoiceByValue("", true);

					inputs.currStatus.val(result.currentStatus); 
				}
				else
				{
					console.error("No valid data received from server.");
				}
			})
			.catch(ex => console.error("Fetch error:", ex));
	}
	catch (ex) {
		console.error(ex);
	}
}

const reset = () => {
	inputs.status.clearStore();
	inputs.status.clearChoices();
	inputs.status.clearInput();
	inputs.status.removeActiveItems();
}

function edit() {
	//try {
	//	const dto = {
	//		Id: inputs.hdnAccId.val(),
	//		FirstName: inputs.fname.val(),
	//		LastName: inputs.lname.val(),
	//		Username: inputs.uname.val(),
	//		Email: inputs.email.val(),
	//		AccountType: inputs.type.getValue().value,
	//		PasswordResetType: getSelectedPasswordOption(),
	//		Password: inputs.pass1.val(),
	//		RetypePass: inputs.pass2.val(),
	//	};

	//	console.log("Dto Sent:", dto);

	//	fetch(window.baseUrl, {
	//		method: 'PUT',
	//		headers: {
	//			'Content-Type': 'application/json'
	//		},
	//		body: JSON.stringify(dto)
	//	})
	//		.then(response => response.json())
	//		.then(data => {
	//			if (data.isSuccess) {
	//				console.log("Edit successful:", data);
	//				$("#modal-edit").modal("hide");

	//				setTimeout(() => {
	//					location.reload();
	//				}, 500);
	//			}
	//			else {
	//				console.error("Edit failed:", data.message);
	//				alert("Edit failed: " + data.message);
	//			}
	//			console.log("Edit successful:", data);
	//		})
	//		.catch(ex => {
	//			console.error("Error during PUT request:", ex);
	//			alert("Something went wrong.");
	//		});
	//}
	//catch (ex) {
	//	console.error("Exception in submitEdit:", ex);
	//}
}

$(document).ready(function () {
	$('#form-edit').submit(function (event) {
		event.preventDefault();
		edit();
	});
});