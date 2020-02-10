---
external help file: Microsoft.Azure.PowerShell.Cmdlets.Cdn.dll-Help.xml
Module Name: Az.Cdn
online version: https://docs.microsoft.com/en-us/powershell/module/az.cdn/set-azcdnwafpolicy
schema: 2.0.0
---

# Set-AzCdnWafPolicy

## SYNOPSIS
Creates or updates a CDN WAF Policy.

## SYNTAX

### ByFieldsParameterSet (Default)
```
Set-AzCdnWafPolicy -PolicyName <String> -ResourceGroupName <String> [-Tag <Hashtable>] [-Disable]
 [-Mode <PSPolicyMode>] [-DefaultRedirectUrl <String>] [-DefaultCustomBlockResponseStatusCode <Int32>]
 [-DefaultCustomBlockResponseBody <String>] [-RateLimitRule <PSRateLimitRule[]>]
 [-ManagedRuleSet <PSManagedRuleSet[]>] [-CustomRule <PSCustomRule[]>]
 [-DefaultProfile <IAzureContextContainer>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### ByResourceIdParameterSet
```
Set-AzCdnWafPolicy -ResourceId <String> [-Tag <Hashtable>] [-Disable] [-Mode <PSPolicyMode>]
 [-DefaultRedirectUrl <String>] [-DefaultCustomBlockResponseStatusCode <Int32>]
 [-DefaultCustomBlockResponseBody <String>] [-RateLimitRule <PSRateLimitRule[]>]
 [-ManagedRuleSet <PSManagedRuleSet[]>] [-CustomRule <PSCustomRule[]>]
 [-DefaultProfile <IAzureContextContainer>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### ByObjectParameterSet
```
Set-AzCdnWafPolicy -CdnWafPolicy <PSPolicy> [-Tag <Hashtable>] [-Disable] [-Mode <PSPolicyMode>]
 [-DefaultRedirectUrl <String>] [-DefaultCustomBlockResponseStatusCode <Int32>]
 [-DefaultCustomBlockResponseBody <String>] [-RateLimitRule <PSRateLimitRule[]>]
 [-ManagedRuleSet <PSManagedRuleSet[]>] [-CustomRule <PSCustomRule[]>]
 [-DefaultProfile <IAzureContextContainer>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
The **Set-AzCdnWafPolicy** cmdlet updates an Azure Content Delivery Network (CDN) Web Application Firewall (WAF) policy.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-AzCdnWafPolicy -ResourceGroupName example-group -PolicyName ExamplePolicy -DefaultRedirectUrl "https://example.com" -CustomRule $customRule1, $customRule2 -ManagedRuleSet $ruleSet
```

Sets a CDN WAF policy named `ExamplePolicy` in the resource group `example-group` with a redirect URL
`https://example.com`, two custom rules, and a managed rule set.

## PARAMETERS

### -CdnWafPolicy
The CDN WAF Policy object.

```yaml
Type: Microsoft.Azure.Commands.Cdn.Models.WebApplicationFirewall.PSPolicy
Parameter Sets: ByObjectParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -CustomRule
Custom rules to apply to the policy.

```yaml
Type: Microsoft.Azure.Commands.Cdn.Models.WebApplicationFirewall.PSCustomRule[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DefaultCustomBlockResponseBody
The HTTP response body used for requests blocked by a rule that doesn't specify a block response body.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DefaultCustomBlockResponseStatusCode
The redirect URL used for requests redirected by a rule that doesn't specify a redirect URL.

```yaml
Type: System.Nullable`1[System.Int32]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DefaultProfile
The credentials, account, tenant, and subscription used for communication with Azure.

```yaml
Type: Microsoft.Azure.Commands.Common.Authentication.Abstractions.Core.IAzureContextContainer
Parameter Sets: (All)
Aliases: AzContext, AzureRmContext, AzureCredential

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DefaultRedirectUrl
The HTTP response status code used for requests blocked by a rule that doesn't specify a block response status code.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Disable
Disable the policy.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ManagedRuleSet
Rule sets managed by CDN to apply to the policy.

```yaml
Type: Microsoft.Azure.Commands.Cdn.Models.WebApplicationFirewall.PSManagedRuleSet[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Mode
Whether the policy prevents or just reports violations.

```yaml
Type: System.Nullable`1[Microsoft.Azure.Commands.Cdn.Models.WebApplicationFirewall.PSPolicyMode]
Parameter Sets: (All)
Aliases:
Accepted values: Prevention, Detection

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PolicyName
Azure CDN WAF policy name.

```yaml
Type: System.String
Parameter Sets: ByFieldsParameterSet
Aliases: Name

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RateLimitRule
Rules to rate limit matched requests.

```yaml
Type: Microsoft.Azure.Commands.Cdn.Models.WebApplicationFirewall.PSRateLimitRule[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ResourceGroupName
The resource group of the CDN WAF Policy will be created in.

```yaml
Type: System.String
Parameter Sets: ByFieldsParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ResourceId
ResourceId of the CDN WAF Policy

```yaml
Type: System.String
Parameter Sets: ByResourceIdParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Tag
The tags to associate with the CDN WAF policy.

```yaml
Type: System.Collections.Hashtable
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: (All)
Aliases: wi

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

### Microsoft.Azure.Commands.Cdn.Models.WebApplicationFirewall.PSPolicy

## OUTPUTS

### Microsoft.Azure.Commands.Cdn.Models.WebApplicationFirewall.PSPolicy

## NOTES

## RELATED LINKS