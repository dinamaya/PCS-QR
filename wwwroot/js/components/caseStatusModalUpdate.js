let inputs = {
	currStatus: null,
	status: null,
	statSelect: null,
	comments: null,
	commentContainer: null,
	hdnCaseId: null
};

let $linkCase = $(`#link-case`);

export function initModal(
	currStatusId,
	statusChoiceInstance,
	statusSelectId,
	commentsId,
	hdnCaseIdElemId,
) {
	inputs.currStatus = $(`#${currStatusId}`);
	inputs.status = statusChoiceInstance;
	inputs.statSelect = $(`#${statusSelectId}`);

	inputs.comments = $(`#${commentsId}`);
	inputs.hdnCaseId = $(`#${hdnCaseIdElemId}`);
	inputs.commentContainer = inputs.comments.parent().parent().parent().parent();
	

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
					$linkCase.attr('href', `/Cases/Update/${caseId}`);
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
	inputs.commentContainer.addClass("d-none");
	inputs.status.clearStore();
	inputs.status.clearChoices();
	inputs.status.clearInput();
	inputs.status.removeActiveItems();
}

function edit() {
	try {
		const dto = {
			CaseId: inputs.hdnCaseId.val(),
			Comments: inputs.comments.val(),
			StatusId: inputs.status.getValue().value,
		};

		const _url = window.baseUrl + "/status"
		console.log("Url:", _url);
		console.log("Dto Sent:", dto);

		fetch(_url, {
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

function checkIsCommentable(selectedStatus) {
	try {
		const _url = `/api/ops/status/commentable?id=${encodeURIComponent(selectedStatus)}`;
		console.log("Url:", _url);

		fetch(_url)
			.then(response => response.json())
			.then(data => {
				console.log("Result: ", data);

				if (data.result) {
					inputs.commentContainer.removeClass("d-none");
				}
				else
				{
					inputs.commentContainer.addClass("d-none");
				}
			})
			.catch(ex => console.error("Fetch error:", ex));
	}
	catch (ex) {
		console.error("Exception in checkIsCommentable:", ex);
	}
}

$(document).ready(function () {
	$('#form-update-stat').submit(function (event) {
		event.preventDefault();
		edit();
	});


	$(inputs.statSelect).on("change", function () {
		checkIsCommentable(this.value);
	});
});