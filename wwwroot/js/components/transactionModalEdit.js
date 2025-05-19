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
			inputs.hdnTransId.val(result.id);
			inputs.remarks.val(result.remarks);
		}
	);

}

$(document).ready(function () {

});
