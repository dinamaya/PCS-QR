let $btnSearch = null;
let $selectCategory = null;
let $textValue = null;
let $selectValue = null;
let $hiddenValue = null;

let choicesCategory = null;
let choicesValue = null;
let statusList = [];
let agedList = []
let onSearch = false;

export function initComponents(selectCategoryId, btnSearchId, textValueId, selectValueId, hiddenValueId, statuses, agedCounts)
{
  $selectCategory = $(`#${selectCategoryId}`);
  $selectValue = $(`#${selectValueId}`);
  $btnSearch = $(`#${btnSearchId}`);
  $textValue = $(`#${textValueId}`);
  $hiddenValue = $(`#${hiddenValueId}`);
  statusList = statuses;
  agedList = agedCounts;

  choicesCategory = new Choices($selectCategory[0], {
    searchEnabled: false
  });

  choicesValue = new Choices($selectValue[0], {
    searchEnabled: false,
    shouldSort: false
  });

  $selectValue.closest(".col").parent().addClass("d-none");
  $textValue.closest(".col").parent().parent().addClass("d-none");

  $selectCategory.on('change', function () {
    const label = $(this).find("option:selected").text().trim();

    reset();

    const $selectWrapper = $selectValue.closest(".col").parent();
    const $textWrapper = $textValue.closest(".col").parent();

    $selectWrapper.addClass("d-none");
    $textWrapper.addClass("d-none");

    if (label === "Days Aged" || label === "Status")
    {
      $selectWrapper.removeClass("d-none");
      const newChoices = label === "Days Aged" ? agedList : statusList;

      choicesValue.setChoices(newChoices, 'value', 'label', false);
      choicesValue.setChoiceByValue("");
    }

    else if (label && label !== "Select Categories") 
      $textWrapper.removeClass("d-none");
    
  });

  $selectValue.on('change', function () {
    const selectedValue = $(this).val();

    if (selectedValue === "" || selectedValue === null) {
      $hiddenValue.val("");
      return;
    }

    const selected = choicesValue.getValue(true);
    $hiddenValue.val(selected);
  });

  $textValue.on('input', function () {
    $hiddenValue.val($(this).val());
  });

  $('#form-search').submit(function (event) {
    event.preventDefault();

    const params = new URLSearchParams();

    if ($selectCategory.val() == "" || $hiddenValue.val() == "")
      return;

    params.set('c', $selectCategory.val());
    params.set('v', $hiddenValue.val());

    const currentUrl = window.location.origin + window.location.pathname;
    const newUrl = `${currentUrl}?${params.toString()}`;

    window.location.href = newUrl;
  });
}

function reset() {
  $hiddenValue.val("");
  $textValue.val("");
  choicesValue.removeActiveItems();
  choicesValue.clearChoices();
}

$(document).ready(function () {
  const params = new URLSearchParams(window.location.search);
  const searchCategory = params.get("c");
  const searchValue = params.get("v");

  onSearch = true;
  choicesCategory.setChoiceByValue(searchCategory);
  $selectCategory.trigger('change');

  $hiddenValue.val(searchValue)

  const label = $($selectCategory).find("option:selected").text().trim();

  setTimeout(() => {
    if (label === "Status" || label === "Days Aged")
      choicesValue.setChoiceByValue(searchValue);
    else
      $textValue.val(searchValue);
  }, 100);
});