let $btnSearch = null;
let $selectCategory = null;
let $textValue = null;
let $selectValue = null;
let $hiddenValue = null;
let $dateRange = null;
let $hiddenDateRange = null;
let choicesCategory = null;
let choicesValue = null;
let statusList = [];
let agedList = []
let onSearch = false;
let flatpickrInstance = null;

export function initComponents(selectCategoryId, btnSearchId, textValueId, selectValueId, hiddenValueId, dateRangeId, hiddenDateRangeId, statuses, agedCounts) {
    $selectCategory = $(`#${selectCategoryId}`);
    $selectValue = $(`#${selectValueId}`);
    $btnSearch = $(`#${btnSearchId}`);
    $textValue = $(`#${textValueId}`);
    $hiddenValue = $(`#${hiddenValueId}`);
    $dateRange = $(`#${dateRangeId}`);
    $hiddenDateRange = $(`#${hiddenDateRangeId}`);
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

    // Initialize flatpickr
    if (document.querySelector('.datepicker')) {
        flatpickrInstance = flatpickr('.datepicker', {
            mode: "range",
            dateFormat: "Y-m-d",
            onChange: function (selectedDates, dateStr, instance) {
                $hiddenDateRange.val(dateStr);
            }
        });
    }

    $selectCategory.on('change', function () {
        const label = $(this).find("option:selected").text().trim();
        reset();
        const $selectWrapper = $selectValue.closest(".col").parent();
        const $textWrapper = $textValue.closest(".col").parent();
        $selectWrapper.addClass("d-none");
        $textWrapper.addClass("d-none");

        if (label === "Days Aged" || label === "Status") {
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

    // Handle date range input changes
    $dateRange.on('change', function () {
        $hiddenDateRange.val($(this).val());
    });

    $('#form-search').submit(function (event) {
        event.preventDefault();
        const params = new URLSearchParams();

        // Add category and value parameters if they exist
        if ($selectCategory.val() != "" && $hiddenValue.val() != "") {
            params.set('c', $selectCategory.val());
            params.set('v', $hiddenValue.val());
        }

        // Add date range parameter if it exists
        if ($hiddenDateRange.val() != "") {
            params.set('dateRange', $hiddenDateRange.val());
        }

        // If no filters are applied, don't submit
        if (params.toString() === "") {
            return;
        }

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

// Function to handle export with current filters
function exportWithCurrentFilters() {
    const exportData = {
        Category: $selectCategory.val() || null,
        CategoryValue: $hiddenValue.val() || null,
        ServicePartner: null, // Add logic if needed
        SearchTerm: null, // Add logic if needed
        DateRange: $hiddenDateRange.val() || null
    };

    // Send AJAX request to export endpoint
    fetch('/CMS/Export', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(exportData)
    })
        .then(response => {
            if (response.ok) {
                return response.blob();
            }
            throw new Error('Export failed');
        })
        .then(blob => {
            // Create download link
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            a.download = `Cases_Export_${new Date().toISOString().slice(0, 19).replace(/[-:]/g, '')}.xlsx`;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);
        })
        .catch(error => {
            console.error('Export error:', error);
            alert('Export failed. Please try again.');
        });
}

// Make export function available globally
window.exportWithCurrentFilters = exportWithCurrentFilters;

$(document).ready(function () {
    const params = new URLSearchParams(window.location.search);
    const searchCategory = params.get("c");
    const searchValue = params.get("v");
    const dateRangeValue = params.get("dateRange");

    onSearch = true;

    // Restore category selection
    if (searchCategory) {
        choicesCategory.setChoiceByValue(searchCategory);
        $selectCategory.trigger('change');
        $hiddenValue.val(searchValue);

        const label = $($selectCategory).find("option:selected").text().trim();
        setTimeout(() => {
            if (label === "Status" || label === "Days Aged")
                choicesValue.setChoiceByValue(searchValue);
            else
                $textValue.val(searchValue);
        }, 100);
    }

    // Restore date range selection
    if (dateRangeValue) {
        $hiddenDateRange.val(dateRangeValue);
        if (flatpickrInstance) {
            // Parse the date range and set it in flatpickr
            const dates = dateRangeValue.split(' to ');
            if (dates.length === 2) {
                flatpickrInstance.setDate([dates[0].trim(), dates[1].trim()]);
            } else {
                flatpickrInstance.setDate(dateRangeValue);
            }
        } else {
            $dateRange.val(dateRangeValue);
        }
    }
});