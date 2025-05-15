import { download } from '../utils.js';

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
	try {
		const row = button.closest("tr");
		const qrId = row.dataset.qrId;
		const spId = row.dataset.spId;

		hdnQrId.val(qrId);
		hdnSpId.val(spId);

		const _url = `${qrUrl}?id=${qrId}`;

		fetch(_url)
			.then(response => response.json())
			.then(data => {
				console.log(data);
				if (data && data.result) {

					if (data.result) {
						const qrImage = `data:image/png;base64,${data.result}`;
						$("#img-qrCode").attr("src", qrImage);
						$("#txt-spname").html(data.message);
					}
					else {
						$("#img-qrCode").attr("src", "/img/qr_placeholder.png");
					}
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

export function downloadQr() {
	try {
		const qrId = hdnQrId.val();

		const _url = `${qrUrl}?id=${qrId}`;

		fetch(_url)
			.then(response => response.json())
			.then(data => {
				if (data && data.result)
				{
					const qrImage = `data:image/png;base64,${data.result}`;
					$("#img-qrCode").attr("src", qrImage);

					download(qrImage, `QR_${qrId}.png`);
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

export function resetQr() {
	setTimeout(() => {
		hdnQrId.val("");
		hdnSpId.val("");
		$("#img-qrCode").attr("src", "/img/qr_placeholder.png");
	}, 800);
}