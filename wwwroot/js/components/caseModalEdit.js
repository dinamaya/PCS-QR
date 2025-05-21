import { httpGet, confirmAction, resetNotifs, addChoicesSelectEvent, showErrorModal, showErrorSimpleModal, httpPut, httpDelete } from "../utils.js";

let inputs = {
	sn: null,
	sp: null,
	hdnCaseId: null
};


let sp = {
	elem: null,
	choices: null
};

let _spId = ""

const notifs = {
	SerialNumber: null,
	ServicePartner: null,
};

export function initModalNotifs(
	snId,
	spId,
) {
	notifs.SerialNumber = $(`#${snId}`);
	notifs.ServicePartner = $(`#${spId}`);

	console.log(notifs);
}

export function initModal(
	snId,
	spId,
	hdnCaseElemId
) {
	_spId = spId;

	inputs.sn = $(`#${snId}`);
	inputs.sp = $(`#${spId}`);
	inputs.hdnCaseId = $(`#${hdnCaseElemId}`);
	sp.elem = inputs.sp.get(0);
	sp.choices = new Choices(sp.elem,
	{
		searchEnabled: true,
		shouldSort: false,
		duplicateItemsAllowed: false,
		removeItemButton: true
	});

	console.log(sp);
	console.log(inputs);
}

function onEdit()
{
	const _url = `${window.caseUrl}?id=${inputs.hdnCaseId.val()}`

	httpGet(
		_url,
		"Case Not Found",
		"There was a problem while fetching the case",
		(response) => {
			const result = response.result;
			inputs.sn.val(result.serialNumber);
			sp.choices.setChoices([{
				value: result.servicePartner,
				label: result.servicePartner,
			}], 'value', 'label', true);
			sp.choices.setChoiceByValue(result.servicePartner);
		}
	);
}


function edit(e, errorTitle, errorDescription, notifList)
{
	if (!e.isConfirmed) return;

	resetNotifs(notifs);

	const dto = {
		Id: inputs.hdnCaseId.val(),
		SerialNumber: inputs.sn.val(),
		ServicePartner: sp.choices.getValue().value
	};

	httpPut(
		window.caseUrl,
		dto,
		'modal-edit-case',
		errorTitle,
		errorDescription,
		notifList
	);
}

function autoSuggest(name)
{
	if (name.length < 3) return;

	const _url = `${window.fetchUrl}?name=${encodeURIComponent(name)}`;
	console.log(_url)

	fetch(_url)
		.then(response => response.json())
		.then(data => {
			if (!data || !data.isSuccess) return;

			sp.choices.clearChoices(); 
			sp.choices.setChoices(
				data.result.map(item => ({
					value: item.value,
					label: item.label
				})),
				'value',
				'label',
				true
			);
		});
}

$(document).ready(function ()
{
	$('#btn-edit-case').click((e) => onEdit());

	addChoicesSelectEvent(_spId, 'Enter Service Partner Name...', (e) => autoSuggest(e.target.value));

	$('#form-edit-case').submit(function (e) {
		e.preventDefault();
		confirmAction(
			'Edit',
			'Edit Case?',
			'This will edit the current case with the provided details!',
			"Case Edit Failed",
			"There was a problem while editing the case.",
			notifs,
			edit
		);
	});
});