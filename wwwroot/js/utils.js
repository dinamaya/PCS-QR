export function displayErrors(errors, notifElems) {
  for (const spans of Object.values(notifElems)) {
    spans[0].innerText = "";
  }

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
