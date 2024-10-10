$(function () {

    async function loadUsersTable(){
        let response = await fetch('/UserRolesTable');

        let userRolesTable = await response.text();
        $(".admin__table").html(userRolesTable);
    }

    loadUsersTable();
});