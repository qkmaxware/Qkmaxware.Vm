const vscode = require('vscode');

const QkasmHoverProvider = require('./src/QkasmHoverProvider');
const QkasmCodeCompletionProvider = require('./src/QkasmCodeCompletionProvider');

/**
 * @param {vscode.ExtensionContext} context
 */
function activate(context) {
    const QKASM_MODE = "qkasm";

    context.subscriptions.push(vscode.languages.registerHoverProvider(QKASM_MODE, new QkasmHoverProvider.QkasmHoverProvider()));
    context.subscriptions.push(vscode.languages.registerCompletionItemProvider(QKASM_MODE, new QkasmCodeCompletionProvider.QkasmMacroCompletionProvider(), "!"));
}

// This method is called when your extension is deactivated
function deactivate() {}

module.exports = {
	activate,
	deactivate
}
