const instruction_list = require("./QkasmInstructionList").Instructions;

function getUsageString(instr) {
    var str = "usage: " + instr.alias;
    for (var i = 0; i < instr.args.length; i++) {
        str = str + " " + instr.args[i].type + "(" + instr.args[i].name + ")"
    }
    return str;
}

// https://code.visualstudio.com/api/references/vscode-api#HoverProvider
class QkasmHoverProvider {
    provideHover(document, position, token) {
        var range = document.getWordRangeAtPosition(position, new RegExp("((\\.@!)?\\w+)"));
        if (range.isEmpty) {
            return null;
        }

        var word = document.getText(range);
        var instr = instruction_list[word];
        if (instr) {
            return {
                contents: [
                    `${instr.opcode} ${instr.alias}`,
                    instr.description,
                    getUsageString(instr)
                ]
            }
        } else {
            var all_text = document.getText();
            var all_vars = Array.from(all_text.matchAll(/@(\w+)\s*=.*$/gm));
            
            var index = -1;
            if (all_vars) {
                for (var i = 0; i < all_vars.length; i++) {
                    var match = all_vars[i];
                    if(match.length > 1) {
                        var var_name = match[1];
                        if (var_name == word) {
                            index = i;
                            break;
                        }
                    }
                }
            }
            
            if (index != -1) {
                return {
                    contents: [
                        `constant pool index ${index}`,
                        all_vars[index][0]
                    ]
                };
            } else {
                return {
                    contents: [word]
                };
            }
        }
    }
};

module.exports = {
    QkasmHoverProvider
}