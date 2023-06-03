using JSBindTool.Core;

if (args.Length == 0)
    throw new Exception("output path is required.");
Binder.CollectTypes();
Binder.GenerateBindings(args[0]);
