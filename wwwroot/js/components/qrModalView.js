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

export function showEdit(qrId, spId) {
	try {
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
				alert("Submission successful!");
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

$(document).ready(function () {
	$('#form-qr').submit(function (event) {
		event.preventDefault();
		post();
	});
});
