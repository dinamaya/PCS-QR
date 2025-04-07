let editInputs = {
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
	editInputs.url = window.baseUrl;
	editInputs.spName = $(`#${spNameId}`);
	editInputs.companyName = $(`#${companyNameId}`);
	editInputs.contactPerson = $(`#${contactPersonId}`);
	editInputs.contactNumber = $(`#${contactNumberId}`);
	editInputs.email = $(`#${emailId}`);

	editInputs.hdnSpId = $(`#${hdnSpIdElemId}`);

	console.log(editInputs);
}

export function onEdit(button) {
	const row = button.closest("tr");
	const spId = row.dataset.spId;

	editInputs.hdnSpId.val(spId);
	const _url = `${editInputs.url}?id=${spId}`

	try {
		fetch(_url)
			.then(response => response.json())
			.then(data => {
				const result = data.result;
				if (data && result) {
					editInputs.spName.val(result.name);
					editInputs.companyName.val(result.companyName);
					editInputs.contactPerson.val(result.contactPerson);
					editInputs.contactNumber.val(result.contactNumber);
					editInputs.email.val(result.email);
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
		editInputs.spName.val("");
		editInputs.companyName.val("");
		editInputs.contactPerson.val("");
		editInputs.contactNumber.val("");
		editInputs.email.val("");
		editInputs.hdnSpId.val("");
	}, 800);
}

function edit() {
	try {
		const dto = {
			id: editInputs.hdnSpId.val(),
			name: editInputs.spName.val(),
			companyName: editInputs.companyName.val(),
			contactPerson: editInputs.contactPerson.val(),
			contactNumber: editInputs.contactNumber.val(),
			email: editInputs.email.val()
		};

		fetch(editInputs.url, {
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
