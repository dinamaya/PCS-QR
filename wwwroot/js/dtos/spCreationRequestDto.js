export class spCreationRequestDto {
  constructor(spName, companyName, contactPerson, contactNumber, email) {
    this.Name = spName || null;
    this.CompanyName = companyName || null;
    this.ContactPerson = contactPerson || null;
    this.ContactNumber = contactNumber || null;
    this.Email = email || null;
  }
}

