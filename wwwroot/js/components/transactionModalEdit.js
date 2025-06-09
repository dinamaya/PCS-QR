import { confirmAction, confirmAction2, handleError, resetNotifs, showErrorModal, httpGet, httpPut, httpDelete } from "../utils.js";

let inputs = {
	remarks: null,
	hdnTransId: null,
}

const notifs = {
	Remarks: null,
};

export function initModal(
	remarksId,
	hdnTransElemId
) {
	inputs.remarks = $(`#${remarksId}`);
	inputs.hdnTransId = $(`#${hdnTransElemId}`);

	console.log(inputs);
}

export function initModalNotifs(remarksId)
{
	notifs.Remarks = $(`#${remarksId}`);

	console.log(notifs);
}

export function onEdit(transactionId) {
	resetNotifs(notifs);

	inputs.hdnTransId.val(transactionId);
	const _url = `${window.transUrl}?id=${transactionId}`

	httpGet(
		_url,
		"There was a problem while fetching the transaction details.",
		"Transaction Remarks not found",
		(data) => {
			const result = data.result;
			inputs.hdnTransId.val(transactionId);
			inputs.remarks.val(result.remarks);
		}
	);

}

function edit(e, errorTitle, errorDescription, notifList) {
	if (!e.isConfirmed) return;

	resetNotifs(notifs);

	const dto = {
		id: inputs.hdnTransId.val(),
		remarks: inputs.remarks.val()
	};

	httpPut(
		window.transUrl,
		dto,
		'modal-edit-transaction',
		errorTitle,
		errorDescription,
		notifList,
		() => displaySpinner(),
		() => hideSpinner(),
	);
}

$(document).ready(function ()
{
	$('#form-edit-transaction').submit(function (event) {
		event.preventDefault();
		confirmAction(
			'Edit',
			'Edit Transaction?',
			'This will edit the current transaction with the provided remarks!',
			"Transaction Edit Failed",
			"There was a problem while editing the transaction.",
			notifs,
			edit
		);
	});
});
