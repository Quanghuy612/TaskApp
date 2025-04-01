$(document).ready(function () {

    $("#addTaskRow").click(function () {
        var newRow = `
            <tr class="taskRow">
                <td><textarea type="text" class="form-control taskName" placeholder="Task Name" required ></textarea></td>
                <td><textarea class="form-control taskDescription" placeholder="Task Description"></textarea></td>
                <td><input type="date" class="form-control h-100 taskDueDate" /></td>
                <td class="text-center align-middle"><button type="button" class="btn btn-danger btn-sm removeTaskRow">Remove</button></td>
            </tr>
        `;
        $("#tasksTable tbody").append(newRow);
    });

    $(document).on('click', '.removeTaskRow', function () {
        $(this).closest('tr').remove();
    });

    $("#createProjectForm").submit(function (e) {
        e.preventDefault();

        const projectName = $("#projectName").val();
        const projectDescription = $("#projectDescription").val();

        let tasks = [];

        $(".taskRow").each(function () {
            const taskName = $(this).find(".taskName").val().trim();
            const taskDescription = $(this).find(".taskDescription").val().trim();
            const dueDate = $(this).find(".taskDueDate").val().trim();

            if (taskName) {
                tasks.push({
                    Title: taskName,
                    Description: taskDescription,
                    DueDate: dueDate
                });
            }
        });

        const projectData = {
            Name: projectName,
            Description: projectDescription,
            TaskList: tasks
        };

        $.ajax({
            url: "/Projects/CreateProject",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(projectData),
            success: function (data) {
                if (data.success) {
                    $('#createProjectModal').modal('hide');
                    location.reload(); 
                } else {
                    alert("Error: " + data.message);
                }
            },
            error: function (xhr, status, error) {
                alert("An error occurred.");
            }
        });
    });

    $('.toggleTasksBtn').click(function () {
        var targetDropdown = $(this).data('target');
        $(targetDropdown).fadeToggle(300);
    });
});
