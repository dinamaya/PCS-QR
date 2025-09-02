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
                        // Try to parse error response as JSON
                        return response.json().then(errorData => {
                            throw new Error(errorData.error || `Export failed: ${response.status} ${response.statusText}`);
                        }).catch(() => {
                            // Fallback if error response is not JSON
                            throw new Error(`Export failed: ${response.status} ${response.statusText}`);
                        });
                    }
                    return response.json();
                })
                .then(data => {
                    if (data.error) {
                        throw new Error(data.error);
                    }

                    // Decode Base64 string and create blob
                    const byteCharacters = atob(data.fileBytes);
                    const byteNumbers = new Array(byteCharacters.length);
                    for (let i = 0; i < byteCharacters.length; i++) {
                        byteNumbers[i] = byteCharacters.charCodeAt(i);
                    }
                    const byteArray = new Uint8Array(byteNumbers);
                    const blob = new Blob([byteArray], { type: data.fileType });

                    // Create download link
                    const url = window.URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = url;
                    a.download = data.fileName;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();

                    // Revoke URL after a short delay
                    setTimeout(() => window.URL.revokeObjectURL(url), 100);

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

    // Gets the search term from the simple-datatable search input.
    function getDataTableSearchTerm() {
        const table = document.getElementById('tbl');
        if (!table) {
            console.log('Export: Could not find table #tbl');
            return null;
        }

        // The simple-datatables library creates a wrapper div around the table.
        // The most reliable way to get the search term is to find the input within this specific table's wrapper.
        const wrapper = table.closest('.dataTable-wrapper');
        if (!wrapper) {
            console.log('Export: Could not find .dataTable-wrapper for the table.');
            return null;
        }

        const searchInput = wrapper.querySelector('.dataTable-input');
        if (searchInput && searchInput.value) {
            console.log('Export: Found search term in table wrapper:', searchInput.value);
            return searchInput.value.trim();
        }

        console.log('Export: Could not find .dataTable-input within the table wrapper.');
        return null;
    }
});