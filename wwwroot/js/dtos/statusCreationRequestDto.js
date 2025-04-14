export class statusCreationRequestDto {
  constructor(name, isCommentable) {
    this.Name = name || null;
    this.IsCommentable = isCommentable || false;
  }
}

