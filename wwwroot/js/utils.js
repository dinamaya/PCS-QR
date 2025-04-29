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

export function handleError(error, title, defaultMessage, notifs)
{
  try
  {
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

export function handleUpdateError(data, title, defaultMessage, notifs) {
  try {
    const response = data.responseText;
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


export function isNullOrEmpty(text) {
  return text == "" || text == null;
}