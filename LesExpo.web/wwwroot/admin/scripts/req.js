// function deleteItem(url, id, dt = true) {
//     console.log('triggered')
//     if (!confirm("Are you sure you want to delete this category?")) {
//         return;
//     }



//     $.ajax({
//         url: url + "/Delete",
//         type: 'DELETE',
//         data: {
//             id: id
//         },
//         success: function (res) {
//             if (res.success) {
//                 // $('#row-' + id).fadeOut(300, function () {
//                 //     $(this).remove();

//                 //     if (dt) {
//                 //         // Optional: Refresh the DataTable to maintain proper pagination
//                 //         dataTables.ajax.reload(null, false); // null, false keeps the current pagination page
//                 //     }

//                 //     toastr.success(res.message);
//                 // });
//                 dataTables.ajax.reload(null, false);




//             } else {
//                 toastr.error(res.message);
//             }
//         },
//         error: function (xhr, status, error) {
//             console.error('Delete error:', error);
//             toastr.error("An error occurred while trying to delete.");
//         }
//     });
// } 




// req.js
function deleteItem(entity, id) {
   Swal.fire({
       title: 'Delete Confirmation',
       text: "Are you sure you want to delete this item?",
       icon: 'warning',
       showCancelButton: true,
       confirmButtonColor: '#d33',
       cancelButtonColor: '#3085d6',
       confirmButtonText: 'Yes, delete it!'
   }).then((result) => {
       if (result.isConfirmed) {
           $.ajax({
               url: `/admin/${entity}/delete/${id}`,
               type: 'DELETE',
               success: function (response) {
                   if (response.success) {


                       // Refresh the DataTable
                       dataTables.ajax.reload();

                       toastr.success(response.message || 'Item deleted successfully');
                   } else {
                       toastr.error(response.message || 'Failed to delete item');
                   }
               },
               error: function (xhr, status, error) {
                   console.error('Delete error:', error);
                   toastr.error('An error occurred while deleting');
               }
           });
       }
   });
}


