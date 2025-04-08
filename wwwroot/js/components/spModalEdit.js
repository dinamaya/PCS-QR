let inputs = {
	spName : null,
	companyName : null,
	contactPerson : null,
	contactNumber : null,
	email: null,
	hdnSpId: null,
	url : "",
}

export function initModal(
	spNameId,
	companyNameId,
	contactPersonId,
	contactNumberId,
	emailId,
	hdnSpIdElemId
) {
	inputs.url = window.baseUrl;
	inputs.spName = $(`#${spNameId}`);
	inputs.companyName = $(`#${companyNameId}`);
	inputs.contactPerson = $(`#${contactPersonId}`);
	inputs.contactNumber = $(`#${contactNumberId}`);
	inputs.email = $(`#${emailId}`);

	inputs.hdnSpId = $(`#${hdnSpIdElemId}`);
}

export function onEdit(button) {
	const row = button.closest("tr");
	const spId = row.dataset.spId;

	inputs.hdnSpId.val(spId);
	const _url = `${inputs.url}?id=${spId}`

	try {
		fetch(_url)
			.then(response => response.json())
			.then(data => {
				const result = data.result;
				if (data && result) {
					inputs.spName.val(result.name);
					inputs.companyName.val(result.companyName);
					inputs.contactPerson.val(result.contactPerson);
					inputs.contactNumber.val(result.contactNumber);
					inputs.email.val(result.email);
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

export function resetModal() {
	setTimeout(() => {
		inputs.spName.val("");
		inputs.companyName.val("");
		inputs.contactPerson.val("");
		inputs.contactNumber.val("");
		inputs.email.val("");
		inputs.hdnSpId.val("");
	}, 800);
}

function edit() {
	try {
		const dto = {
			id: inputs.hdnSpId.val(),
			name: inputs.spName.val(),
			companyName: inputs.companyName.val(),
			contactPerson: inputs.contactPerson.val(),
			contactNumber: inputs.contactNumber.val(),
			email: inputs.email.val()
		};

		fetch(inputs.url, {
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

//function updateTableRow(dto) {
//	const row = $(`tr[data-sp-id='${dto.id}']`);
//	row.children[2].text(dto.name);
//	row.children[3].text(dto.companyName);
//	row.children[4].text(dto.contactNumber);
//	row.children[5].text(dto.email);
//}

$(document).ready(function () {
	$('#form-edit').submit(function (event) {
		event.preventDefault();
		edit();
	});
});
