import { download, httpGet, httpPost } from '../utils.js';

var hdnQrId = null;
var hdnSpId = null;

var qrUrl = "";

export function initQrModal(qrBaseUrl) {
	qrUrl = qrBaseUrl;

	hdnQrId = $("#hdn-qrId");
	hdnSpId = $("#hdn-spId");
}

export function viewQr(button)
{
	const row = button.closest("tr");
	const qrId = row.dataset.qrId;
	const spId = row.dataset.spId;

	hdnQrId.val(qrId);
	hdnSpId.val(spId);

	const _url = `${qrUrl}?id=${qrId}`;

	httpGet(
		_url,
		"Generating QR Code Failed",
		"There was a problem while generating the QR code of the current service partner.",
		(response) => {
			console.log("QR response data:", response);
			const qrImage = `data:image/png;base64,${response.result}`;
			$("#img-qrCode").attr("src", qrImage);
			$("#txt-spname").html(response.message);
		},
		() => {
			$("#img-qrCode").attr("src", "/img/qr_placeholder.png");
		}
	);
}

export function downloadQr() {
	const qrId = hdnQrId.val();
	const _url = `${qrUrl}?id=${qrId}`;

	httpGet(
		_url,
		"Downloading QR Code Failed",
		"There was a problem while downloading the QR code of the current service partner.",
		(response) => {
			const qrImage = `data:image/png;base64,${response.result}`;
			$("#img-qrCode").attr("src", qrImage);
			download(qrImage, `QR_${qrId}.png`);
		}
	);
}

export function resetQr() {
	setTimeout(() => {
		hdnQrId.val("");
		hdnSpId.val("");
		$("#img-qrCode").attr("src", "/img/qr_placeholder.png");
	}, 800);
}

export function emailQr() {
	const spId = hdnSpId.val();
	const dto = { spId: spId };

	httpPost(
		'/api/qr/email',
		dto,
		'modal-qr',
		'Emailing QR Code Failed',
		'There was a problem while emailing the QR code.',
		null,
		null,
		null,
		() => {
			Swal.fire({
				title: 'Email Sent!',
				text: 'The QR code has been sent to the service partner.',
				icon: 'success'
			});
		}
	);
}