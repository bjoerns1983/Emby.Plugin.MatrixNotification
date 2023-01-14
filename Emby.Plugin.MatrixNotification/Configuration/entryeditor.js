define(['globalize', 'pluginManager', 'emby-input'], function (globalize, pluginManager) {
    'use strict';

    function EntryEditor() {
    }

    EntryEditor.setObjectValues = function (context, entry) {

        entry.FriendlyName = context.querySelector('.txtFriendlyName').value;
        entry.Options.ClientToken = context.querySelector('.txtMatrixClientToken').value;
        entry.Options.RoomID = context.querySelector('.txtMatrixRoomID').value;
		entry.Options.ServerAddress = context.querySelector('.txtMatrixServerAddress').value;
    };

    EntryEditor.setFormValues = function (context, entry) {

        context.querySelector('.txtFriendlyName').value = entry.FriendlyName || '';
        context.querySelector('.txtMatrixClientToken').value = entry.Options.ClientToken || '';
        context.querySelector('.txtMatrixRoomID').value = entry.Options.RoomID || '';
		context.querySelector('.txtMatrixServerAddress').value = entry.Options.ServerAddress || '';
    };

    EntryEditor.loadTemplate = function (context) {

        return require(['text!' + pluginManager.getConfigurationResourceUrl('matrixeditortemplate')]).then(function (responses) {

            var template = responses[0];
            context.innerHTML = globalize.translateDocument(template);

            // setup any required event handlers here
        });
    };

    EntryEditor.destroy = function () {

    };

    return EntryEditor;
});