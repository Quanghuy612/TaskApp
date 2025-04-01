$(document).ready(function () {

    $("#createUserForm").submit(function (event) {
        event.preventDefault();

        let submitButton = $("#createUserForm button[type='submit']");
        submitButton.prop("disabled", true);

        let formData = {
            Username: $("#username").val().trim(),
            Password: $("#password").val(),
            FullName: $("#fullName").val().trim(),
            Email: $("#email").val().trim(),
            Role: $("#role").val(),
            ImgPath: $("#imgPath").val().trim(),
        };

        $.ajax({
            url: "/Users/CreateUser",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    alert("User created successfully!");
                    location.reload();
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function () {
                alert("An error occurred while adding the user.");
            },
            complete: function () {
                submitButton.prop("disabled", false); 
            }
        });
    });

    $(document).on("click", ".delete-user", function () {
        let userId = $(this).data("userid");

        if (!confirm("Are you sure you want to delete this user?")) return;

        $.ajax({
            url: `/Users/DeleteUser/${userId}`,
            type: "DELETE",
            success: function (response) {
                if (response.success) {
                    alert("User deleted successfully!");
                    location.reload();
                } else {
                    alert("Failed to delete user: " + response.message);
                }
            },
            error: function () {
                alert("An error occurred while deleting the user.");
            }
        });
    });

    $(document).on("click", ".view-user", function () {
        let userId = $(this).data("userid");

        if (userId) {
            window.location.href = `/Users/Detail/${userId}`;
        } else {
            alert("User ID is missing!");
        }
    });

    $("#updateUserForm").submit(function (event) {
        event.preventDefault();

        let userData = {
            Id: parseInt($("#userId").val(), 10),
            FullName: $("#userFullName").val().trim(),
            Email: $("#userEmail").val().trim(),
            Address: $("#userAddress").val().trim(),
            Role: $("#userRole").val()
        };
        $.ajax({
            url: "/Users/UpdateUser",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(userData),
            success: function (response) {
                if (response.success) {
                    alert("User updated successfully!");
                    location.reload();
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function () {
                alert("An error occurred while updating the user.");
            }
        });
    });
});