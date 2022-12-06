using FluentAssertions;
using Sprache;

namespace Day3;

/*
--- Day 3: Rucksack Reorganization ---

One Elf has the important job of loading all of the rucksacks with supplies for the jungle journey. Unfortunately, that Elf didn't quite follow the packing instructions, and so a few items now need to be rearranged.

Each rucksack has two large compartments. All items of a given type are meant to go into exactly one of the two compartments. The Elf that did the packing failed to follow this rule for exactly one item type per rucksack.

The Elves have made a list of all of the items currently in each rucksack (your puzzle input), but they need your help finding the errors. Every item type is identified by a single lowercase or uppercase letter (that is, a and A refer to different types of items).

The list of items for each rucksack is given as characters all on a single line. A given rucksack always has the same number of items in each of its two compartments, so the first half of the characters represent items in the first compartment, while the second half of the characters represent items in the second compartment.

For example, suppose you have the following list of contents from six rucksacks:

vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw
The first rucksack contains the items vJrwpWtwJgWrhcsFMMfFFhFp, which means its first compartment contains the items vJrwpWtwJgWr, while the second compartment contains the items hcsFMMfFFhFp. The only item type that appears in both compartments is lowercase p.
The second rucksack's compartments contain jqHRNqRjqzjGDLGL and rsFMfFZSrLrFZsSL. The only item type that appears in both compartments is uppercase L.
The third rucksack's compartments contain PmmdzqPrV and vPwwTWBwg; the only common item type is uppercase P.
The fourth rucksack's compartments only share item type v.
The fifth rucksack's compartments only share item type t.
The sixth rucksack's compartments only share item type s.
To help prioritize item rearrangement, every item type can be converted to a priority:

Lowercase item types a through z have priorities 1 through 26.
Uppercase item types A through Z have priorities 27 through 52.
In the above example, the priority of the item type that appears in both compartments of each rucksack is 16 (p), 38 (L), 42 (P), 22 (v), 20 (t), and 19 (s); the sum of these is 157.

Find the item type that appears in both compartments of each rucksack. What is the sum of the priorities of those item types?
*/

public readonly record struct Rucksack(string RucksackContent)
{
    public int Items => RucksackContent.Length;
    public string FirstCompartment => RucksackContent.Substring(0, Items / 2);
    public string SecondCompartment => RucksackContent.Substring(Items / 2);
    public IEnumerable<char> CommonItems => FirstCompartment.Intersect(SecondCompartment);
    public int CommonItemsPriority =>
        CommonItems
            .Select(c => (int)c)
            .Select(cInt => 'A' <= cInt && cInt <= 'Z'
                ? cInt - (int)'A' + 27
                : cInt - (int)'a' + 1)
            .Sum();
}

public static class RucksackParser
{
    public static Parser<Rucksack> Rucksack =
        from rucksackContent in Parse.Letter.Many().Text()
        select new Rucksack(rucksackContent);
}

public class RucksackParserTests
{
    public const string PuzzleExample =
"""
vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw
""";
    public const string PuzzleInput =
"""
sfDRhjhHsHhgWPJvPmmQnmPqnW
pTddGVwcpMTTCdnQJqqQqqqVtVms
MdZCZGdcrCNRFZRhFssL
CttWnSnNfSnCHsWrTlTPPpPCTRrLpl
DgqqghjqJBVgDMTPGVlRGwbfLLGP
cgqBBhjqcBdMcWQcQNnNzsfv
lnDWMgTLlTFlHHgDDgngWFnlBWNcBQrdjcrrdQrPBrdjhWhj
JqSVRRVmmRqJJbZGGJqJvbmBNcjPNQNssQPhSSdwPwwwQr
bCRJqGJJmzmJZRCmFNTLTttTzfFfLglf
SPWvWMvCSPcjzjDbcwfjTl
lLNRNLqhhQVQJlRjrjrDwTzzqzzfrb
GRnRVhRJLFnnhtJQNVdLdLgWCmmZlMlgSCSWSgpZtPBM
pTGFrLFTFWFprLDBmLbSbtmBDb
MqjwqJwZlqJjHlqjHHPmSbsffDmsStDnHnQmsm
ZPJjVPZbVMRRPZwMJZVMNJMcGWpWFcWFFNFGrWTzWzFrzG
MffZZtMTnTtSZLdfgSMtCHSbmWsGwbHGSqvmCqWb
lzpQhrhphhlzDDhRPmBvqHGRbBbwbbssCB
JJljpvhFrrjhptnddMJfdtgnMT
drCtpNLCLpTpJSdswQhvDbHZHDLDHQ
WmWgBWRcRzVVWVBgBBnnlfgWHjmvjQhwbbbshQvZDQQjsHZC
fqBzggWPPzBWBzffcfnMJdtFtrpqrGMpdCCTdM
JwJWqNBNNdzzBSzGsqbdNJbVMpptPmZMrVZrrZMtPmPwDp
THgfgffffHRhQRLVMGVQmGtLDGmM
TjGchhlHhGfhRHgRgWJSqzJWWlqNSzsWSJ
dNmPlzdvdspsFWwQmG
bhZSbVJBJnLNTnwWVHMGwQsGMFFw
RnnbbTnSnSSTTnLfRCCPqDPDNDlfCfDDcr
lhhTcnPchPPHCCStwWTHbS
GDRFNqlQJsGJqGJDqVNsqssDQBSZWHQBCZHHwbZbtHtwCbZW
RNJrFJFJDrmqsVjNmDvrvfzfffcvdpMrvlfh
DtLdNGHNfwBJQwgCrncgpSpcnlfC
sGqWPMPTvPPhTjjsqRqPvSlzFFpjnnSrjczprgllFF
vVPGPGMbPGqTRWsMqZhqvbZNLLmLQddQdmBtBwBNBwNB
ChVzhwpdpqHhtNmHHNHt
QsjGTQcTWQjfjbssQDPmHgfrrVrPZnntZD
jTGJSGvWJwqlvlCBqV
pRVcSRffTPfBWfNVfWBWdJdwhvvwGjjFmGvhLTdh
qsrHqtbDZqsnsZqCQDtHnQQLwFvJFhGJwddvwLcCwJJJdv
sgcqHnzqqzgnHnqrstZzqsnSPllRlVVSNpWVNVRgMBlVPW
WRQTtHrTrrDRvQDHrbtJlpdhLdGsDllfspLpphhs
GzCSqCSmSmVSpsljphlpsL
gVwCVGzmNmCNRQTvJQJHnvwQ
psBDsswNjBcqtHtsTHsqtM
vQrPqZPmvgQZrfgmPrfJQlLvnLzVHLnSnnTLTnnHMt
PRCJRPgmqrmZmmqQQDDRwwNwjwDwpFjDDW
VtBgCqbVjPbSbHtPRdrssZMFZlrRsBRw
LzWmhcDqTDvnDWTFMrwRvwFrGZdGlZ
zJLczJDnWDpDNzmDczWLzWzWNCbbttHPCHbSVbqgHSCjffQf
TjTfvJjjvcjTQcDzMDfQTLLbLgVVVhMrWWblghbbLN
dZHFSpqpqpbWrhhlWh
dFwPBHqFSqwZSmZmSlqZjTvvJTmzcJsTTTjfvsfQ
qqqNTlfjzbMGJlHMSZtZzZgRZDgZDzdS
nLCCVVcmgCdZdSlg
cmQscBVpFsppsVlffQGJHjlWHJGq
whwVGGZhVLwhsFFDCTrDccCctrcctL
CzSSvPSzTBStSWND
HllCHvHJPPqjCPvbfdvvbsmhdRRRsmZhsRdMFQMRFQ
gFCfCVfCsLCftsBsDbSHrbJRJJtrmrddrd
hqQpqWhlNlpMlppfdTRhmbmTdbdHJH
NGvvjvpvpfGgGGDCZZBg
rmBtgdddtqmmrqBGbLGJlmctWWvbNzvfpsVVfzzSVSTsWNpz
RPDRjMhDFljvsvzlSs
wDChhnCQwDwmgJclqrgm
WHrrDbWHQPzNrrRVMQJMQGvvsvvjnDLvfsjsvwfGws
dhdhZhcphZZZmtFFTcZSmcZsnfqjLRnngnpwnGfqfvvnRs
FZcZhtmhFCttldFlSSmlSthQJzNVMbRPWWPJzrbbJCNMPH
wMFBpvTppLpwfNfjggmNmGTj
ddSDDbGHnRDQDZRHSZSdRZDQzjjrzNNNfnmNrllhgglhfgWg
bCQqsJqGDZCHbppwvtVMMvJcLL
pSpSVdLDFCvDDvCFvJgwjsJbNtmtJgSjmj
ZcWNNBQfwjsttsbc
WNQMBflQQNGQrFpFVVRDHpCMDR
PfPvqLphWpWLtZSWpWLPjwJbmDwJbbDbmJVjPQ
lQQnRGMllMjswrmwJM
ggRGFFGGdlGGzFFzzFFcNBvSfLQZShZTtdLWWZWSWZSp
lCfgHsVHJDdswNRmsMRQ
vccvvFVrPcvQNRdnmqdR
rctPBrPBTTpPFBLZZcCCgVHJHVjjbLHfCjfS
dfGdsGGrlFFlbWjfgblhJhLDLDDMLNvJNLLBnmLB
tSppwQQHSSVtwStSpZZVqRJmBDzLvwPmzJznBDDmLBBJ
cHvtpRSvptCRbbjrrcfjrWjf
BBdHdjgQdjMMsHJscFnrzLpLgznLFzcF
wvllmNmVvZfvmZWqcPptPztFSWLFGrrFnt
ZbCvqqmNvflfVTbZfNllsjHdjdhhHDTHRBJjjMsc
FNCPtPtgLFJwPwflFwSrLFcMczQZTbMVmzzVZMcNVVVb
DhRDdhpWQDZmzVSQ
vnhBSHppjRBHqpWvrPFtJLJlLvfLPF
nmcSnnWjmfCTcHPHJCvh
zdDdlrrzGFFLPtPhBBhH
NGNGrzrRrzphwwwMmqqfnsfZZNbSjQSN
lgznQGWQLQWlnSzHSQlwnlDhCbZhZhZChPChwDcDphcb
jTRvVVrMvmLCPNcNZhRNcD
jfftvsrVJLsVvJqsmfqjfjlQgzlQWFzHFGGBQgtFgnnH
sllpwssrsCwrTRgCHGCTcnZD
jjzJtSdhdzbJWhdQqLdzqSHmDZBGZmmcGGgBGTDRBQTD
SVjgVhgtbVzJPfFpvvNrNswV
StzdmmnnjSRRdhPPdZZd
VbTbCqFFMbZTFcNQLgRgQbvvRh
pGsqGGGfHGfZVffzwtrHnmJHllznrS
NLWJvtLjtLzBjNSvSMDCHfwHSlDMlSSHfZ
RTPTVmhpnprfcfgZwgRD
PnPGFhGsTphsFpdPnpVdmhwFvBJzbdWNtJJjtNJNjbvJtttN
RvmgjDqqjqRgZRMRDpQjQhWsbPLPFnPFFbVVLbdSbnPSvP
NwczHBrJTzcBJHrfWJBCJcrCdnPPPNSlLnsnbFnnLlbSFddS
rGJwCCCJHwBGGctGDtphQQMppQWZmRpD
RPhhSMqRccBDZPPPRhPcNZSzzTLJrWZLmVVQLWZdTQQJWL
nwggfwCvbjwvbwpzWLpWVLdrrrQVTm
vgnGGCsCtntbFsgqlRVMSqNVBDtPSc
mtstjJmvTNBcjRRCHCfH
gLpglwwlgHbZbgpgFrdBBBfdfSPBLSSrcS
GQGglGWWgMglQFHgbmTmNtDqnDDVJMDMNJ
ZMbBZfvVfFfBbMvfMhgbfDsrSTTszcldmTTPmcPFDz
QqQQnwrqWQpwRWWpWwJRwNzTTSPpzPPdTPpSPmdSscmS
GjjtJRWtwGQjRZVChZMjVCrMMf
fJNPTvDPTpHHTPwvjNNHDfTWthhgQQGdBddtlvMsMQMvQh
rFbZVZrLmLdGrrhMBQWg
FmnzVRFLqVqqVLVRRFZSFmTwfHHjHCNCCDGwjnCDDfNH
gQHHQJgCnNJnQFQPRbDQzLRR
mwrdpctWtrMvvrrWwGMmGWWPLzFFLSbLnDFsLPdDFbZRLz
vGcmGwBBMGtmmrvlMrGlqNghlVjCCnTHHCHgCCjh
LmLvVjVjsrmrtmmr
tfcnbScRnlMZtHQPCgSQssPdHC
RGGGGnRfcwnGbbJRBRcwJfnGtBDhhVptNhDvLLhVvvjBWNvT
dZWNQZgQbbNvdWGgZvbTfLrjtrPlGJfrLqLJlj
TMmDpwzmVMHpBLfrcccMfqLjct
SwnSBTDDTwwzwnnsFSZdRNbQZWRvgSCvbQ
WPgZgQLLbMgdBrdnGqqfdhVVvR
HzssNTzwlwHHcczwFjMFHjVGrqRqnVThVqrGrGTRqvrf
zFzcHFNlzJBLgMQLJCZL
nPLNcWtNtlLMccLlWdTjzzbBfBQSzqzBqPqS
RbbDZZrGRJhJjgJQSjCfqQCC
rrmRbDDwvZDpprbGrbDvtctlVVVHvdtlcMtWHMHV
DWrZJrQjWwFcrhzVzbpmpcVqhb
MFnFHMNSqbMpMMmG
FNngNRBRCgnHCCHRPvLNdgJWwJDlJJDssZDLWWlWQlsl
BQqNsGrbBCNbNCrMpGpbHhthRCDRDRJCmDVRhRJP
nfvWvcnSWncSTdzzFLJtRmhHmPPVPVTwwHHtTh
WfLfnfSJZJvdLFZWngfBMGMppGMrNBbGMpZjrj
rccMjBMVJcjjjNNqmmCf
LLspTTGsTGntsntTFwnNNfFqQmmNgNqfNQmZvQ
tpDTwlGDTGPPsbtsLsnnqGTJzJrJBzHzMVrMRzBMlShVBR
PsrNPRjjPbjzjLRWLbjmvtCnMntnpfmtNZNCNv
dDlfwwJllhJTcllScSCQvmtCnmtCmQmQmG
TTFcdhJwhBFfwJJHhdchVclrsbWsbzqLgbzrrjgVRgsqgW
vvcvvDJFcDZPTzwfcwSLczzScz
VNnnVVsqGNntsqtBRblqBndSfzCCRzwRfCHSjdfSjzSH
pppsMVlGGhhrZwMMDP
LltNHMZNHMfNnfgtLHWWbhWjcblSbVbcTWVP
vFmCZsqRRBqrVPWsWTWPWb
mQBqJRdqQBqQzzRQztgLgntGZttddLMggw
ZTCCrCWfGLGBWSwHvHHmHvmTTH
bllhnsbjDlqFfqjhnFRppwmvJppmpRRwMNSmmw
FlnFDjdtqhDdfZZBrtBrrPLt
CRCTHHJcCmJgTSTRcSMcRMVstssSrtprppVFtdrdspNb
jjllnvgBLqdsGprtqtFG
vQjzWnWZWjBLhjgwcccRJgZPCmJm
VRNmBBRNRFcCRcFVRSVSqZLLvvlLqvLfzfMhjJLC
TdHsHbDsbHMJLqlLzl
bgQGsgWWGGgbDgwGzBNSFrFtVSmwRRNFtn
pCCggQPPzWnvlDcWVHGJcNBl
LhsLMrwwGlnMBlNG
mmhwZmqSLwjLttnFbvgFTpPtPtgFCz
TtZSJzFZhZzTFcgFFcmRRmJJQllCHvPshVQsCrshsCssHVHW
GjGGDGqdGfbpDBjMdjpBjBNbVHtsWWPHlMlrrrrWWlVlVsCs
dBdDdfqLdBjjFRFScRStmLnL
GtVppGGPbVgTVFQrZzfrJfJJtMJr
DslmNmLsnmNHNNnnqQRZSJSQfqrJzSJn
BNljDHsHlvhmBshDljWsDWlHdgvpVTFggVgGcTTpvFPTzGCV
GRcnTRtcQTcBTsNtpvhFCmmFhZvFPC
bBJMgqWfdwBJfMPPPmvPqhmjvvPC
SMJMdJbdfwJgVglMWWVdcQnBzSQDzGGQzRQDTQSB
mvjVzLgTzVzvVjJrJgrlMhZRFTtRlRhMRRRtFZ
HGqnNNqfnHNGGfCHndBqnqfFlcppsJMZplMFpMtlscRlpC
qSnPGqqbnSdVrvQrrSJjSV
lWFSWZZvVqnqfnSrJzMcPDjJBJcBMPFJ
NGppNgHdHbRsHPbsgGspTwHTMcmMDdJMMmzBDcMBMDQmjMBd
TGGLRGwHsGtpHgHpNbpttwrvCnvCrqSSLvWqPqSSnWvn
jwcqBNNdZLjSfvPdddRlfb
CDVmsgMHCnnDnhVghmDnDCzRRrSrbrlTbsSTlzzlvzPb
gCCFmCWDnChGCFHnGCLBcwwjvZQZNtGqqNZc
LBDcNstdNJscccVDhLHNDHVtFvdldlFvCSnSvjSSbblgvZjF
rWznQqGMMrmmRZbbwvSFgjwbwm
RQnTQfWqqTzTLJJLVtBTsc
SvwCTHqCqqqHtwtnnHHDtWgrBQLzzVLLzSQVFhbrSFLL
cZmPNmPJdmPjPdcclRPPdhBCFVVVrQzQCCLbcgVbBV
fNlmfZfpfWMCtGGpnM
bSNssNssbPHVccPhclPGpP
ffQfZdZZBDDZgLvhmhzVmVppmlpGgh
jdQQQJRljSFFTWCT
lvlLtvnhnfvMgtrvWjmTmPPzjHcrmdcjdd
qCbssCJbppQZQbRJDQSZCJRpzhmcQjdcTBmmGzmdcmjGmdmT
SqwSbJZSpwwFJFDDbqtNVMwVMMlVNgNVlLhV
DqGFQGNMGMQwCcgtCJcr
sVfjWlzzVsmzVZsdVlHrhjppcgpjrhpphcSJ
LRdLsZBWWmlZldZRmzPDvDTTDMGTPFPvBTTc
jzzzpjgBzTDQQHPH
gLLtZVdCdsLfnbZCbdZtHDfHTJJPPmJJfmHQDJqD
bVtWndLtcZgnhsvMSBrMFrvBWNrB
sfqhLDcqfqRRqQhQRqMcvlJpJwFgzwpjplwbgpwzLz
CrGttnhTWtmSnGrtTtSCZGFzbgHHFFFjljHjZHHFwgwl
mBnrrTmWWCGStVCmMcDPcPBqcsRhcvPR
GLZLBNrGZdGGVgMVJVhnvn
dmWlcqcQMWCJVhMn
cdpPqtQbcHlmQjmZswFfTRFpBTfwwT
ZhtZpvbnbpPbtLHLvdsNdcRLNd
jDDjlCflGwsHfdrfTLrrdN
MzmljBMBWPtsbtSQtW
GHrzPSrNLFnMtSBZjZBB
WWbfDmVmwmmlbVDldWslNnBMJJNZZJCtJJJn
vwDfffVvmDwdTvDRQvpLNpLpcRFpphhHLPHg
scsTslgcnCTCScSTcqLLWlFWLLqbGvRbpL
NZMBdBPtNbbrLGqqqGvqZF
NttdbhMPfjQfNtbMbMmNjhNcCzczSSCSJTSzTCScnfnzwC
pjdjCGGGWPCMSDfS
JhFMFcrgBHPnSnWFWDDn
HVBBJctBccghsJhgrbwLGTTdtjLdbmTMMb
DtGHgDPfGfPhfLwNWSSJQcpHcr
dvlMCzdnMRFCCTjnZNpNQJcSbrWzrpSQWS
TVvFJJMjJdlMvRvMClllZZgPtPGsftfDqtGfVGsGtqqq
jSmmcjmJqcBgwmWMCLLzCsMz
TnTQVDGQTpZGNQHDZDHHQDwsCCdLrflsrCVzVrwWzwrr
zDFpppnNQtnTQQvZZZNvnhqqjtRccRbgqqbSSSjPRg
FwClNSwCFstWZLDLvhvjvtjhhD
TmsHmsmrggzmqnnGGvPGjTbbRGBhbB
cHVqgcrVzrQqzHmMcrMnczzcWFVCCFNJZWJZswwFCZWwffwS
mzbsmbmLRCZTRbSJFvPLPJPJpJffcP
QqWqNVNNNllnnWTglqTVlGNPJDvwcJpwfwccPgccPJDfJF
HMGnNMltqGMjHGqMzmTSmzTsRSszSm
qlGDfljllCTgqCTvCDfBHHQsbrSZZHSHWtvWZB
NzpnNpRnLLwRpdwpVhtqQbSbsWQWbSWnrrnH
cFqwFNpLdVcDJlDgccTD
BRqjnSBNBpRHHpjpBSnHnRBQfQzzCvzWrsWCTvfsvCsCCsfC
ZMVbhqbMdlbLTdsWvfPdPC
hlZVDMZcwJNSgjJgJFnq
CZwZssQQZrmsCmNNDpDGFblclD
HMjWMbBVfnnbMbnzMpFhlNSNFFSDcDGSzN
LnLLqjnBMjMngHbnWrTgZsCsgZvvvQrvQs
RCFCCJQbCQcprRlHHPpHhd
tWWLwvswfvZshgqDpdpBgfdf
mZtvZtMpjZzwWFjJTcQQbjjnSQ
fBfVwtttLDFctDtwFPWfTppWfmHCHdJhdChT
bGMRsbsvMQSSzMzZSNzsZvRNWTZJlmgZTJJdhhmppHTJCgTg
jssjNSSGMsQHbsRvHNPjtDcDcLPPPPDwLDDV
pClhQjJccrpbpqHhMhVhSMqHPt
dBZGZdgBzRsBsvMwGGVPVqMGwtVH
ZvDddZvDBdDdDmgCmVmbbCNpCCbljW
DTMCpdCnwRDwdfMCDDCssfZmGrBrjpttjrNrgctmGpGr
VVqJQgSSWzhPGGrPtNNQtm
bFvhgWzHJlDdffswTvRd
jwCCPPTtCswCCNTsqRNbMqQMVvVzMMMQSGvQqn
hprHlmFcHcdhWWLchZzHrLMvvnBvJJSBJMVMnnmMnMMJ
WppLcZdHWHplZWlDHhHTfzRzCCsTTtNNgtjgDw
vhmDFcDZmczMrwcqrMrmDFrvggtVSWgtSNwsjBtNVSnBsjsS
dbbRJHbpCWBBpZVgSS
LZLdHlClPmqLGDvMDv
mFbWsvsJVtbbRwfTSP
BGpQllhLGqhplBGZBfLMTSTLwwfwMJwMPT
GlDnDpQZlZZpZBlpWDNcmrgrWmNdNJvc
zbtqTtHQbZZpqbPpvGJdvQdhrhQjdQGs
qDFLLSNqcWwsGhGDJh
LgBcfnFCSFnLccggSVCVtHZlpqPPtTRMftHMbMzb
hzrrWnzRZRnbWVRzjcRHMDdqqQdNMHqHQQjlHM
sGCpCtppBfCTgwBBCwPBCssQqMQvNlSMMQDQNqHGHvDSbQ
tpLFPgfbCsfbzzcnJhRhZLhc
qzzGqfpFvWFmRSPjPjRP
cwwVssBMtNMNLngstgVBnrsPmHSJJmjllhQdQldmhdrjQJ
nDVSsLwcVcMnBGzTDDCvpvfzGT
bcTbbcZGZLPgTMWZpLLDQnrvPVnVmmjmRPFVrF
HJCJqlzBdsSjzCJRmlrlrnVQQDFnVF
BfwffNdNswLtbWbNcpjt
smJwSNNFMzFNDrvbrbfJHvbl
BRQjqZQcBhrbTsbTnfcn
ZLQRZRBjjPWSsmCdSWMgSN
NhwlDpbWggdSBvBggLFg
fRrZsVfjqljmsQQVmmsnFMFSBLLRvFTFMBSvFF
QfqVVzcsQmcQqrcsNwzzzPphHlwNppPH
nnFdsjVdmpBsBVFHzjpvlTfQdPcQQPGPcvlGPv
DWMDCCWbNJhLtMgJMNLgtMgQflZQlfQGjZZhQZGhTfQcQP
rCrtJJgLLMbgDgMDWNRrWRnzsjpFzBzSHmmqHmqnHH
rmjjJmmdwSmGhdsjJtsgGNzFWQFnBFVWHdFQcLLcNz
RCCbfRlvvPfvCTnHLLnNbNLczHnQ
lqZTllRRpDMlpfZRvgQpSmwwtggQjJgtpS
LDsGvTSSsswCwTrLZDqQWHMWbphlHMpGGpQz
RRPfPRccBdVjPcFlpMpMQWzMWfpF
RjPRjRtczcNBJRSCtLDTvTSDCCST
pqQNgNnSntwgqzzQCzNwCNBRcWtBjZcZGrBMcHMGvWcr
mmJdJPFVbJbPPGZbMRbvvrjcMj
lTMVVlLPfLNQhpgqLSLn
HlBHFrgBvlfzFzqvnvFqpCJbJfQpQpLcmhbcmtmm
jDjPGsRRTMMPjdJmjmLpCLth
MRMZMWsNpFFFVFHW
RGgwWcppGSWcWSRWmGdWcttHQFJHfbQwBQJTJQBQfJ
njjZZCMlCZjqMBFbJQZHJHBQft
DsjCPDDvjFNsMNjNqpGspcsGSmcpccrGWS
cVwMZGVZwHNPgPwRZwHttThlHllvlzQpptzppl
DsCWdqLdDCnfJLSCqsqWRsBdlhjlhzlttzQhhtvlhnhhhbzT
JCWWRWCrLDDdBdLsSsLLSCrCNZMVcmMZMFwMZwNZPZVGFPmr
hhPzDzPhPNbfpzhBbdNbDhttzqWtwttHWwntjqmwmWFm
LgGZSdMMrgTLrZLdgLSgsGTFFjrWtFFmmmFtWjqHFnFtjn
vZgdLvZLZQLRQZQQdMZLdQvVpRhNNPfJDbcBbbhVNJNNhf
""";

    [TestCase("vJrwpWtwJgWrhcsFMMfFFhFp", "vJrwpWtwJgWr")]
    [TestCase("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "jqHRNqRjqzjGDLGL")]
    [TestCase("PmmdzqPrVvPwwTWBwg", "PmmdzqPrV")]
    public void FirstCompartmentTests(string input, string expected) =>
        RucksackParser
            .Rucksack.Parse(input)
            .FirstCompartment.Should().Be(expected);

    [TestCase("vJrwpWtwJgWrhcsFMMfFFhFp", "hcsFMMfFFhFp")]
    [TestCase("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "rsFMfFZSrLrFZsSL")]
    [TestCase("PmmdzqPrVvPwwTWBwg", "vPwwTWBwg")]
    public void SecondCompartmentTests(string input, string expected) =>
        RucksackParser
            .Rucksack.Parse(input)
            .SecondCompartment.Should().Be(expected);

    [TestCase("vJrwpWtwJgWrhcsFMMfFFhFp", new[] { 'p' })]
    [TestCase("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", new[] { 'L' })]
    [TestCase("PmmdzqPrVvPwwTWBwg", new[] { 'P' })]
    [TestCase("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", new[] { 'v' })]
    [TestCase("ttgJtRGJQctTZtZT", new[] { 't' })]
    [TestCase("CrZsJsPPZsGzwwsLwLmpwMDw", new[] { 's' })]
    public void CommonItemTests(string input, char[] expected) =>
        RucksackParser
            .Rucksack.Parse(input)
            .CommonItems.Should().BeEquivalentTo(expected);

    [TestCase("vJrwpWtwJgWrhcsFMMfFFhFp", 16)]
    [TestCase("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", 38)]
    [TestCase("PmmdzqPrVvPwwTWBwg", 42)]
    [TestCase("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", 22)]
    [TestCase("ttgJtRGJQctTZtZT", 20)]
    [TestCase("CrZsJsPPZsGzwwsLwLmpwMDw", 19)]
    public void CommonItemsPriorityTests(string input, int expected) =>
        RucksackParser
            .Rucksack.Parse(input)
            .CommonItemsPriority.Should().Be(expected);
}
