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

