let $btnSearch = null;
let $selectCategory = null;
let $textValue = null;
let $selectValue = null;
let $hiddenValue = null;
let $dateRange = null;
let $hiddenDateRange = null;
let $dateRangeContainer = null;
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
    $dateRangeContainer = $('#dateRangeContainer');
    statusList = statuses;
    agedList = agedCounts;

    const $selectWrapper = $selectValue.closest(".col");
    const $textWrapper = $textValue.closest(".col");
    const $btnWrapper = $btnSearch.closest(".col-12");

    choicesCategory = new Choices($selectCategory[0], {
        searchEnabled: false
    });
    choicesValue = new Choices($selectValue[0], {
        searchEnabled: false,
        shouldSort: false
    });

    // Hide all components initially, except for the category dropdown
    $selectWrapper.addClass("d-none");
    $textWrapper.addClass("d-none");
    $dateRangeContainer.addClass("d-none");
    $btnWrapper.addClass("d-none");

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
        $selectWrapper.addClass("d-none");
        $textWrapper.addClass("d-none");
        $dateRangeContainer.addClass("d-none"); // Hide datepicker by default
        $btnWrapper.removeClass("d-none");
        $("#pnl-search").removeClass("d-none");


        if (label === "Out of SLA") {
            // For "Out of SLA", only show date picker
            $dateRangeContainer.removeClass("d-none");
            $hiddenValue.val(""); // Explicitly clear value for "Out of SLA"
        } else if (label === "Days Aged") {
            // For "Days Aged", show select dropdown for values
            $selectWrapper.removeClass("d-none");
            choicesValue.setChoices(agedList, 'value', 'label', false);
            choicesValue.setChoiceByValue("");
        } else if (label === "Status") {
            // For "Status", show select dropdown for values AND date picker
            $selectWrapper.removeClass("d-none");
            choicesValue.setChoices(statusList, 'value', 'label', false);
            choicesValue.setChoiceByValue("");
            $dateRangeContainer.removeClass("d-none");
        } else if (label === "Service Partner Name") {
            // For "Service Partner Name", show text input AND date picker
            $textWrapper.removeClass("d-none");

            // Show datepicker for Service Partner
            if (label === "Service Partner Name") {
                $dateRangeContainer.removeClass("d-none");
            }
        } else if (label && label !== "Select Categories" && label !== "") {
            // For other simple text-based categories (e.g., "Serial Number", "Case Number / ID", "Remarks / Comment")
            $textWrapper.removeClass("d-none"); // Show text input field
        } else {
            // "Select Categories" or empty label, ensure value is cleared
            $hiddenValue.val("");
            $btnWrapper.addClass("d-none");
            $("#pnl-search").addClass("d-none");
        }


        // Reset date range visual and hidden field if the current category does not use it.
        if (label !== "Status" && label !== "Service Partner Name" && label !== "Out of SLA") {
            $hiddenDateRange.val("");
            // $dateRange.val(""); // flatpickrInstance.clear() handles the visible input
            if (flatpickrInstance) {
                flatpickrInstance.clear();
            }
        }
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


        if ($selectCategory.val() != "" && $hiddenValue.val() == "") {
            params.set('c', $selectCategory.val());
            params.set('v', "");
        }

        // Add category and value parameters if they exist
        else if ($selectCategory.val() != "" && $hiddenValue.val() != "") {
            params.set('c', $selectCategory.val());
            params.set('v', $hiddenValue.val());
        }

        // Add date range parameter if it exists and is allowed for the selected category
        const selectedLabel = $selectCategory.find("option:selected").text().trim();
        if ($hiddenDateRange.val() != "" && (selectedLabel === "Status" || selectedLabel === "Service Partner Name" || selectedLabel === "Out of SLA")) {
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
    const selectedLabel = $selectCategory.find("option:selected").text().trim();
    const dateRangeValue = (selectedLabel === "Status" || selectedLabel === "Service Partner Name" || selectedLabel === "Out of SLA") ? $hiddenDateRange.val() || null : null;

    const exportData = {
        Category: $selectCategory.val() || null,
        CategoryValue: $hiddenValue.val() || null,
        ServicePartner: getServicePartnerFromUrl(),
        SearchTerm: getDataTableSearchTerm(),
        DateRange: dateRangeValue
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

// Helper functions
function getServicePartnerFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('d') || null;
}

function getDataTableSearchTerm() {
    const searchSelectors = [
        'input[type="search"]',
        '.dataTables_filter input',
        '.dataTable-search',
        '.dataTable-input',
        '.simple-datatables-search'
    ];

    for (const selector of searchSelectors) {
        const searchInput = document.querySelector(selector);
        if (searchInput && searchInput.value && searchInput.value.trim()) {
            return searchInput.value.trim();
        }
    }
    return null;
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

    // Restore date range selection only if allowed for the selected category
    if (dateRangeValue && searchCategory) {
        const label = $($selectCategory).find("option:selected").text().trim();
        if (label === "Status" || label === "Service Partner Name" || label === "Out of SLA") {
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
    }
});