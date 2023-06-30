const vscode = require('vscode');
const instruction_list = require("./QkasmInstructionList").Instructions;

// https://code.visualstudio.com/api/references/vscode-api#CompletionItemProvider
class QkasmMacroCompletionProvider {
    provideCompletionItems(document, position, token, context) {
        var items = [];
        for (var key in instruction_list) {
            var instr = instruction_list[key];
            if (instr.alias.startsWith("!")) {
                items.push(new vscode.CompletionItem({label: instr.alias.substring(1), description: instr.description}, vscode.CompletionItemKind.Function))
            }
        }
        return items;
    }
}

module.exports = {
    QkasmMacroCompletionProvider
}