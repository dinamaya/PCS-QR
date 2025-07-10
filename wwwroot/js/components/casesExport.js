document.addEventListener('DOMContentLoaded', function () {
    const exportButton = document.getElementById('btnExportExcel');
    const table = document.getElementById('tbl');
    const antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

    if (exportButton && table) {
        exportButton.addEventListener('click', function () {
            // Show loading indicator
            exportButton.disabled = true;
            const originalText = exportButton.textContent;
            exportButton.textContent = 'Exporting...';

            // Get current filter parameters from the URL or form
            const urlParams = new URLSearchParams(window.location.search);
            const requestData = {
                Category: urlParams.get('c') || null,
                CategoryValue: urlParams.get('v') || null,
                ServicePartner: urlParams.get('d') || null,
                SearchTerm: getDataTableSearchTerm(),
                DateRange: urlParams.get('dateRange') || null  // Added missing date range
            };

            console.log('Export request data:', requestData); // Debug log

            // Call the ExportAll endpoint
            fetch('/CMS/Export', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': antiForgeryToken
                },
                body: JSON.stringify(requestData)
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`Export failed: ${response.status} ${response.statusText}`);
                    }
                    return response.blob();
                })
                .then(blob => {
                    // Create download link
                    const url = window.URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = url;
                    a.download = `Cases_Export_${new Date().toISOString().replace(/[:.]/g, '')}.xlsx`;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                    window.URL.revokeObjectURL(url);

                    console.log('Export completed successfully');
                })
                .catch(error => {
                    console.error('Export error:', error);
                    alert('Failed to export cases. Please try again.');
                })
                .finally(() => {
                    // Reset button state
                    exportButton.disabled = false;
                    exportButton.textContent = originalText;
                });
        });
    }

    // Enhanced function to get DataTable search term
    function getDataTableSearchTerm() {
        // Method 1: Check for Simple DataTables instance
        if (window.dataTable && window.dataTable.input) {
            const searchValue = window.dataTable.input.value;
            if (searchValue && searchValue.trim()) {
                console.log('Search term from DataTable API:', searchValue);
                return searchValue.trim();
            }
        }

        // Method 2: Try to get from various search input selectors
        const searchSelectors = [
            'input[type="search"]',                    // Standard search input
            '.dataTables_filter input',                // DataTables wrapper
            '.dataTable-search',                       // Simple DataTables
            '.dataTable-input',                        // Simple DataTables input
            '.simple-datatables-search',               // Simple DataTables search
            '[data-search]',                           // Custom search attribute
            '#dataTable_filter input',                 // Specific ID
            '.search-input',                           // Generic class
            'input[placeholder*="Search"]',            // Input with Search in placeholder
            'input[placeholder*="search"]',            // Input with search in placeholder
            'input[aria-label*="Search"]',             // ARIA label with Search
            'input[aria-label*="search"]'              // ARIA label with search
        ];

        for (const selector of searchSelectors) {
            const searchInput = document.querySelector(selector);
            if (searchInput && searchInput.value && searchInput.value.trim()) {
                console.log(`Search term from selector "${selector}":`, searchInput.value);
                return searchInput.value.trim();
            }
        }

        // Method 3: Try to find the search input within the table's parent container
        const tableContainer = table.closest('.dataTables_wrapper, .dataTable-wrapper, .table-container');
        if (tableContainer) {
            const searchInput = tableContainer.querySelector('input[type="search"], input.search, .search-input');
            if (searchInput && searchInput.value && searchInput.value.trim()) {
                console.log('Search term from table container:', searchInput.value);
                return searchInput.value.trim();
            }
        }

        // Method 4: Check if there's a global DataTable variable
        if (typeof $ !== 'undefined' && $.fn.DataTable) {
            const dtInstance = $(table).DataTable();
            if (dtInstance && dtInstance.search) {
                const searchTerm = dtInstance.search();
                if (searchTerm && searchTerm.trim()) {
                    console.log('Search term from jQuery DataTable:', searchTerm);
                    return searchTerm.trim();
                }
            }
        }

        console.log('No search term found');
        return null;
    }
});