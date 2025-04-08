import { qrCreationRequestDto } from '../dtos/qrCreationRequestDto.js';

// '#edit-description'
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

export function generateQr() {
	try {
		const dto = hdnSpId.val();

		$.post({
			url: qrUrl,
			contentType: "application/json",
			data: JSON.stringify(dto),
			success: function (response) {
				console.log("Submission successful:", response);
				const button = $(`[data-sp-id='${dto}']`);
				button.attr("data-qr-id", response.result.qrId);
				hdnQrId.val(response.result.qrId);

				const qrImage = `data:image/png;base64,${response.result.qrImage}`;
				$("#img-qrCode").attr("src", qrImage);
			},
			error: function (error) {
				console.error("Submission failed:", error);
				alert("Submission failed!");
			}
		});
	}
	catch (ex) {
		console.error(ex);
	}
}

export function downloadQr() {
	try {
		const _url = `${qrUrl}?id=${qrId}`;

		$.post({
			url: qrUrl,
			contentType: "application/json",
			data: JSON.stringify(dto),
			success: function (response) {
				if (data && data.result) {
					if (data.result) {
						const qrImage = `data:image/png;base64,${data.result}`;

						$("#img-qrCode").attr("src", qrImage);

						const link = document.createElement('a');
						link.href = qrImage;
						link.download = `QR_Code(${qrId}).png`;
						link.style.display = 'none';

						document.body.appendChild(link);
						link.click();

						document.body.removeChild(link);
					} else {
						$("#img-qrCode").attr("src", "/img/qr_placeholder.png");
					}
				} else {
					console.error("No valid data received from server.");
				}
			},
			error: function (error) {
				console.error("Submission failed:", error);
				alert("Submission failed!");
			}
		});
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