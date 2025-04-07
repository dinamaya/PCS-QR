export class accountCreationRequestDto
{
  constructor(firstName, lastName, username, email, type, password1, password2) {
    this.FirstName = firstName || null;
    this.LastName = lastName || null;
    this.Username = username || null;
    this.Email = email || null;
    this.Type = type || null;
    this.Password = password1 || null;
    this.RetypePass = password2 || null;
  }
}
