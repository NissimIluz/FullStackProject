export interface PeriodicElement {
    sharedWithUser: documentDTO[];
    ownDocuments: shareDTO[];
}

export interface documentDTO {
    documentID: string;
    documentName: string;
}

export interface shareDTO {
    documentID: string;
    documentName: string;
    sharedWithUsers: string[];
}

export interface dataTable {
    documentId: string;
    documentName: string;
    sharedUsers: string;
    addShare: string;
    removeDocument: string;
}

