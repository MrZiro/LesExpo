function initializeDataTable(entity, columns, tableId = '#tblData') {
    const $table = $(tableId);
    const baseUrl = `/admin/${entity}`;

    // Create thead and headers
    const $thead = $(`${tableId} thead`);
    const $headerRow = $('<tr>').appendTo($thead);

    // Add columns including the Actions column
    columns.concat({ title: 'Actions', width: '' }).forEach(col => {
        $headerRow.append(
            $('<th>').css('width', col.width).text(col.title)
        );
    });

    // Initialize DataTable with modified options
    dataTables = $table.DataTable({
        ajax: { url: `${baseUrl}/getall` },
        order: [], // Prevents default sorting
        createdRow: function (row, data, dataIndex) {
            console.log(data);
            console.log(row);
            // Add id and class to each row
            $(row).attr('id', 'row-' + data.id);
            $(row).addClass('data-row');
        },
        columns: columns.concat({
            data: 'id',
            render: function (data) {
                // Use 'edit' action for Blog entity, keep 'upsert' for others
                const editAction = entity === 'Blog' ? 'edit' : 'upsert';
                return `<div class="btn-container" role="group">
                    <a href="${baseUrl}/${editAction}?id=${data}" class="btn btn-mine1 edit-btn">
                        <i class="bi bi-pencil-square"></i>
                    </a>
                    <button class="btn btn-mine- delete-btn" 
                            onclick="deleteItem('${entity}', '${data}')">
                        <i class="bi bi-trash-fill"></i>
                    </button>
                </div>`;
            },
            width: "",
            orderable: false
        }),
        //processing: true,
        // pageLength: 10,
        // processing: true,
        serverSide: true,
        language: {
            zeroRecords: "Eşleşen kayıt bulunamadı",
            info: "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            paginate: {
                next: "Sonraki",
                previous: "Önceki"
            },
            search: "Arama:",
            lengthMenu: "_MENU_ kayıt göster"
        },
        pagingType: "full_numbers",
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "Tümü"]],
    });
}


function initializeDataTableNoOption(entity, columns, tableId = '#tblData') {
    const $table = $(tableId);
    const baseUrl = `/admin/${entity}`;

    const $thead = $(`${tableId} thead`);
    const $headerRow = $('<tr>').appendTo($thead);

    columns.concat({ title: 'Actions', width: '' }).forEach(col => {
        $headerRow.append(
            $('<th>').css('width', col.width).text(col.title)
        );
    });

    dataTables = $table.DataTable({
        ajax: { url: `${baseUrl}/getall` },
        order: [], // Prevents default sorting
        createdRow: function (row, data, dataIndex) {
            console.log(data);
            console.log(row);
            $(row).attr('id', 'row-' + data.id);
            $(row).addClass('data-row');
        },
        columns: columns.concat({
            data: 'id',
            render: function (data) {
                // Use 'edit' action for Blog entity, keep 'upsert' for others
                const editAction = entity === 'Blog' ? 'edit' : 'upsert';
                return `<div class="btn-container" role="group">
                    <a href="${baseUrl}/${editAction}?id=${data}" class="btn btn-mine1 edit-btn">
                        <i class="bi bi-pencil-square"></i>
                    </a>
                    <button class="btn btn-mine1 delete-btn" 
                            onclick="deleteItem('${entity}', '${data}')">
                        <i class="bi bi-trash-fill"></i>
                    </button>
                </div>`;
            },
            width: "",
            orderable: false
        }),
        language: {
            zeroRecords: "Eşleşen kayıt bulunamadı",
            info: "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            search: "Arama:",
            lengthMenu: "_MENU_ kayıt göster"
        },
        paging: false,
        info: false,
        searching: false,
        ordering: false

    });


}

$(document).on('init.dt', function(e, settings) {
    const table = $(settings.nTable);
    table.closest('.dt-container')
         .find('.dt-layout-row:last')
         .css('margin-top', 'auto').css('display', 'flex').css('height', '12em').css('align-items', 'end');
});







function initializeDataTableWithOption(entity, columns, tableId = '#tblData', filter = null) {
    const $table = $(tableId);
    const baseUrl = `/admin/${entity}`;
    
    // Determine URL based on filter
    let ajaxUrl = `${baseUrl}/getall`;
    if (filter && filter !== 'all') {
        // Use generic getbylanguage endpoint for language filtering
        ajaxUrl = `${baseUrl}/getbylanguage?language=${filter}`;
    }

    // Create thead and headers
    const $thead = $(`${tableId} thead`);
    $thead.empty(); // Clear existing headers to prevent duplication
    const $headerRow = $('<tr>').appendTo($thead);

    // Add columns including the Actions column
    columns.concat({ title: 'Actions', width: '' }).forEach(col => {
        $headerRow.append(
            $('<th>').css('width', col.width).text(col.title)
        );
    });

    // Initialize DataTable with modified options
    dataTables = $table.DataTable({
        ajax: { url: ajaxUrl },
        order: [], // Prevents default sorting
        destroy: true, // Allow reinitialization
        createdRow: function (row, data, dataIndex) {
            console.log(data);
            console.log(row);
            // Add id and class to each row
            $(row).attr('id', 'row-' + data.id);
            $(row).addClass('data-row');
        },
        columns: columns.concat({
            data: 'id',
            render: function (data) {
                // Handle different entity actions
                if (entity.toLowerCase() === 'contactad' || entity.toLowerCase() === 'registrationad' || entity.toLowerCase() === 'ticketad') {
                    return `<div class="btn-container" role="group">
                        <a href="${baseUrl}/details/${data}" class="btn btn-mine1 edit-btn" title="Görüntüle">
                            <i class="bi bi-eye"></i>
                        </a>
                        <button class="btn btn-mine1 delete-btn" 
                                onclick="deleteItem('${entity}', '${data}')" title="Sil">
                            <i class="bi bi-trash-fill"></i>
                        </button>
                    </div>`;
                } else {
                    // Use 'edit' action for Blog entity, keep 'upsert' for others
                    const editAction = entity === 'Blog' ? 'edit' : 'upsert';
                    return `<div class="btn-container" role="group">
                        <a href="${baseUrl}/${editAction}?id=${data}" class="btn btn-mine1 edit-btn">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <button class="btn btn-mine1 delete-btn" 
                                onclick="deleteItem('${entity}', '${data}')">
                            <i class="bi bi-trash-fill"></i>
                        </button>
                    </div>`;
                }
            },
            width: "",
        }),
        language: {
            zeroRecords: "Eşleşen kayıt bulunamadı",
            info: "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            paginate: {
                next: "Sonraki",
                previous: "Önceki"
            },
            search: "Arama:",
            lengthMenu: "_MENU_ kayıt göster"
        },
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "Tümü"]],
    });
    
}

// Function to reload data with language filter
function reloadDataTableWithFilter(entity, columns, tableId, filter) {
    if (dataTables) {
        dataTables.destroy();
    }
    return initializeDataTableWithOption(entity, columns, tableId, filter);
}