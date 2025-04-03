$(document).ready(function () {

    var currentTaskId;

    $('.assign-user-btn').click(function () {
        currentTaskId = $(this).data('taskid');
        $('#assignUserModal').modal('show');
        $('#taskIdToAssign').val(currentTaskId);
    });

    $('#userSearch').keyup(function () {
        var searchTerm = $(this).val();
        if (searchTerm.length > 2) {
            $.get('/Users/Search', { term: searchTerm }, function (users) {
                var resultsHtml = '';
                if (users.length > 0) {
                    users.forEach(function (user) {
                        resultsHtml += `
                        <a href="#" class="list-group-item list-group-item-action user-select" 
                           data-userid="${user.id}">
                            ${user.name} (${user.email})
                        </a>`;
                    });
                } else {
                    resultsHtml = '<div class="list-group-item">No users found</div>';
                }
                $('#userSearchResults').html(resultsHtml);
            });
        } else {
            $('#userSearchResults').html('<div class="list-group-item">Type at least 3 characters</div>');
        }
    });

    $(document).on('click', '.user-select', function (e) {
        e.preventDefault();
        var userId = $(this).data('userid');
        $('#userSearch').val($(this).text().trim());
        $('#taskIdToAssign').data('userid', userId);
    });

    $('#confirmAssign').click(function () {
        var userId = $('#taskIdToAssign').data('userid');
        var taskId = $('#taskIdToAssign').val();

        if (!userId) {
            alert('Please select a user first');
            return;
        }

        $.post('/Tasks/AssignUser', { taskId: taskId, userId: userId }, function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert('Error assigning user: ' + response.message);
            }
        }).fail(function () {
            alert('Error assigning user');
        });
    });

    $('#taskModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var taskId = button.data('taskid');
        var title = button.data('title');
        var description = button.data('description');
        var dueDate = button.data('duedate');
        var projectName = button.data('projectname');
        var userFullName = button.data('userfullname');
        var status = button.data('status');

        var modal = $(this);
        modal.find('#taskId').val(taskId);
        modal.find('#taskTitle').val(title);
        modal.find('#taskDescription').val(description);
        modal.find('#taskProjectName').val(projectName);
        modal.find('#taskUserFullName').val(userFullName);
        modal.find('#taskStatus').val(status);

        if (dueDate && !isNaN(Date.parse(dueDate))) {
            modal.find('#taskDueDate').val(new Date(dueDate).toISOString().split('T')[0]);
        } else {
            modal.find('#taskDueDate').val('');
        }
    });

    $('#saveTaskBtn').click(function () {
        var taskData = {
            Id: $('#taskId').val(),
            Title: $('#taskTitle').val(),
            Description: $('#taskDescription').val(),
            DueDate: $('#taskDueDate').val(),
            Status: $('#taskStatus').val()
        };

        $.ajax({
            url: '/Tasks/Edit',
            type: 'POST',
            data: taskData,
            success: function (response) {
                $('#taskModal').modal('hide');
                alert(response.message);
                location.reload();
            },
            error: function (xhr, status, error) {
                alert('Error saving task');
            }
        });
    });

    $(document).on('click', '.complete-btn', function () {
        var taskId = $(this).data('task-id');
        updateTaskStatus(taskId, 'Completed');
    });

    $(document).on('click', '.cancel-btn', function () {
        var taskId = $(this).data('task-id');
        updateTaskStatus(taskId, 'Cancelled');
    });

    function updateTaskStatus(taskId, status) {
        $.ajax({
            url: '/Tasks/UpdateStatus',
            type: 'POST',
            data: {
                taskId: taskId,
                status: status
            },
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    location.reload();
                } else {
                    alert('Failed to update the status');
                }
            },
            error: function () {
                alert('An error occurred');
            }
        });
    }
});