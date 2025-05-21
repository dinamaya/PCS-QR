export function displayErrors(errors, notifElems) {
  resetNotifs(notifElems);

  for (const [key, messages] of Object.entries(errors)) {
    const notif = notifElems[key]?.[0];
    if (notif && messages.length > 0) {
      notif.innerText = messages[0];
    }
  }
}

export function resetNotifs(notifElems) {
  for (const spans of Object.values(notifElems)) {
    spans[0].innerText = "";
  }
}

export function showErrorModal(message, title, defaultMessage, notifs) {
  resetNotifs(notifs);

  const _message = message || defaultMessage ;
  Swal.fire({
    title: title,
    text: _message,
    icon: "error"
  });

  console.error(title, _message);
}

export function showErrorSimpleModal(message, title, defaultMessage)
{
  const _message = message || defaultMessage;
  Swal.fire({
    title: title,
    text: _message,
    icon: "error"
  });

  console.error(title, _message);
}

export function handleError(error, title, defaultMessage, notifs)
{
  try
  {
    if (error.status == 401)
    {
      showErrorSimpleModal("Please refresh the page", "Session expired")
      return;
    }

    const response = error.responseText;
    const result = JSON.parse(response);
    const errors = result.errors;

    if (!errors)
      throw new DOMException(result.message);

    displayErrors(errors, notifs);
  }
  catch (e) {
    showErrorModal(e.message, title, defaultMessage, notifs);
  }
}

export function handleSimpleError(error, title, defaultMessage) {
  try {
    if (error.status == 401) {
      showErrorSimpleModal("Please refresh the page", "Session expired")
      return;
    }

    const response = error.responseText;
    const result = JSON.parse(response);
    const errors = result.errors;

    if (!errors)
      throw new DOMException(result.message);
  }
  catch (e) {
    showErrorSimpleModal(e.message, title, defaultMessage);
  }
}

export function handleModalError(data, title, defaultMessage, notifs) {
  try {
    const response = data.responseText;
    const result = JSON.parse(response);
    const errors = result.errors;


    displayErrors(errors, notifs);
  }
  catch (e) {
    showErrorModal(e.message, title, defaultMessage, notifs);
  }
}


export function isNullOrEmpty(text) {
  return text == "" || text == null;
}

export function checkErrorResponse(respone) {
  if (respone.status == 401) {
    showErrorSimpleModal("Please refresh the page", "Session expired")
    throw new Error("Session expired"); 
  }

  return respone.json();
}

// Use only on the Put and Post Requests
export function confirmAction(positiveText, title, description, errorTitle, errorDescription, notifList, submitCallback) {
  Swal.mixin({
    customClass: {
      confirmButton: 'btn bg-gradient-success',
      cancelButton: 'btn bg-gradient-danger'
    },
    buttonsStyling: !1
  })
    .fire({
      title: title,
      text: description,
      icon: 'question',
      confirmButtonText: positiveText,
      cancelButtonText: 'Cancel',
      reverseButtons: !0,
      showCancelButton: !0
    })
    .then(e => submitCallback(e, errorTitle, errorDescription, notifList))
    .catch(e => {
      showErrorModal(e.message, errorTitle, errorDescription, notifList);
    });
}

export function confirmAction2(positiveText, title, description, errorTitle, errorDescription, submitCallback) {
  Swal.mixin({
    customClass: {
      confirmButton: 'btn bg-gradient-success',
      cancelButton: 'btn bg-gradient-danger'
    },
    buttonsStyling: !1
  })
    .fire({
      title: title,
      text: description,
      icon: 'question',
      confirmButtonText: positiveText,
      cancelButtonText: 'Cancel',
      reverseButtons: !0,
      showCancelButton: !0
    })
    .then(e => submitCallback(e, errorTitle, errorDescription))
    .catch(e => {
      showErrorSimpleModal(e.message, errorTitle, errorDescription);
    });
}

export function httpGet(url, errorTitle, errorDescription, successCallBack, errorCallBack)
{
  console.log(url);
  $.ajax({
    url: url,
    method: 'GET',
    success: function (response) {
      console.log("Response:", response);
      if (response && response.result)
        successCallBack(response);
      else
      {
        if (errorCallBack) errorCallBack();
        handleSimpleError("Invalid generation", errorTitle, errorDescription);
      }
    },
    error: (error) =>
    {
      handleSimpleError(error, errorTitle, errorDescription);
      if (errorCallBack) errorCallBack();
    }
  });
}

export function httpPut(url, dto, modalId, errorTitle, errorDescription, notifList) {
  console.log(url);
  $.ajax({
    url: url,
    method: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(dto),
    success: function (response) {
      hideModal(modalId);
      refreshPage(window.okEditParam);
    },
    error: (error) =>
      handleError(error, errorTitle, errorDescription, notifList)
  });
}

export function httpPost(url, dto, modalId, errorTitle, errorDescription, notifList) {

  console.log(url);
  $.ajax({
    url: url,
    method: 'POST',
    contentType: 'application/json',
    data: JSON.stringify(dto),
    success: function (response) {
      hideModal(modalId);
      refreshPage(window.okCreateParam);
    },
    error: (error) =>
      handleError(error, errorTitle, errorDescription, notifList)
  });
}

export function httpDelete(url, modalId, errorTitle, errorDescription, notifList) {
  console.log(url);
  $.ajax({
    url: url,
    method: 'DELETE',
    success: function (response) {
      hideModal(modalId);
      refreshPage(window.okDeleteParam);
    },
    error: (error) =>
      handleSimpleError(error, errorTitle, errorDescription)
  });
}


function refreshPage(queryParam) {
  setTimeout(() => {
    const url = new URL(window.location.href);
    url.searchParams.set('q', queryParam);
    window.location.href = url.toString();
  }, 300);
}

function hideModal(modalId) {
  const modalEl = $(`#${modalId}`);
  const modalInstance = bootstrap.Modal.getInstance(modalEl);
  modalInstance.hide();
}

export function download(href, filename) {
  const link = document.createElement('a');
  link.href = href;
  link.download = filename;
  link.style.display = 'none';

  document.body.appendChild(link);
  link.click();

  document.body.removeChild(link);
}

export function addChoicesInputEvent(elemId, eventCallback) {
  $(`#${elemId}`).siblings('.choices__list--multiple').siblings('.choices__input--cloned')
    .on('input', function (e) {
      eventCallback(e);
    });
}


export function addChoicesSelectEvent(elemId, placeHolderValue, eventCallback) {
  const $elem = $(`#${elemId}`).parent()
    .siblings('.choices__list.choices__list--dropdown')
    .find('.choices__input.choices__input--cloned');

  $elem.on('input', eventCallback);
  $elem.attr('placeholder', placeHolderValue);
}


export function onCopy(toastElemId, btnCopyId)
{
  const toast = document.getElementById(toastElemId);
  const copyText = document.getElementById(btnCopyId);

  copyText.select();
  copyText.setSelectionRange(0, 99999);
  navigator.clipboard.writeText(copyText.value);

  const toastBs = bootstrap.Toast.getOrCreateInstance(toast);
  toastBs.show()
}